using BjjTrainer_API.Data;
using BjjTrainer_API.Models.DTO;
using BjjTrainer_API.Models.DTO.Moves;
using BjjTrainer_API.Models.DTO.UserDtos;
using BjjTrainer_API.Models.Trainings;
using Microsoft.EntityFrameworkCore;

namespace BjjTrainer_API.Services_API.Users
{
    public class UserProgressService
    {
        private readonly ApplicationDbContext _context;

        public UserProgressService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<UserProgressDto> GetUserProgressAsync(string userId)
        {
            try
            {
                var userLogs = await _context.TrainingLogs
                    .Where(log => log.ApplicationUserId == userId)
                    .Include(log => log.TrainingLogMoves)
                    .ThenInclude(tlm => tlm.Move)
                    .ToListAsync();

                var trainingGoals = await _context.TrainingGoals
                    .Where(goal => goal.ApplicationUserId == userId)
                    .Include(goal => goal.UserTrainingGoalMoves)
                    .ThenInclude(gtm => gtm.Move)
                    .ToListAsync();

                if (!userLogs.Any() && !trainingGoals.Any())
                {
                    return new UserProgressDto
                    {
                        TotalGoalsAchieved = 0,
                        TotalMoves = 0,
                        TotalTrainingTime = 0,
                        TotalRoundsRolled = 0,
                        TotalSubmissions = 0,
                        TotalTaps = 0,
                        WeeklyTrainingHours = 0,
                        AverageSessionLength = 0,
                        FavoriteMoveThisMonth = "No data available",
                        MovesPerformed = []
                    };
                }

                var totalTrainingTime = userLogs.Sum(log => log.TrainingTime);
                var totalRoundsRolled = userLogs.Sum(log => log.RoundsRolled);
                var totalSubmissions = userLogs.Sum(log => log.Submissions);
                var totalTaps = userLogs.Sum(log => log.Taps);

                var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
                var weeklyTrainingHours = userLogs
                    .Where(log => log.Date >= oneWeekAgo)
                    .Sum(log => log.TrainingTime / 60);

                var averageSessionLength = userLogs.Any() ? userLogs.Average(log => log.TrainingTime) : 0;

                var currentMonth = DateTime.UtcNow.Month;
                var favoriteMove = userLogs
                    .Where(log => log.Date.Month == currentMonth)
                    .SelectMany(log => log.TrainingLogMoves)
                    .Where(tlm => tlm.Move != null) // Ensure Move is not null
                    .GroupBy(tlm => tlm.Move!.Name) // Use null-forgiving operator since we filtered nulls
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key ?? "No moves practiced";

                var movesPerformed = userLogs
                    .SelectMany(log => log.TrainingLogMoves)
                    .GroupBy(tlm => tlm.Move!.Id) // Use null-forgiving operator to suppress nullability warning
                    .Select(group => new MoveDto
                    {
                        Id = group.Key,
                        Name = group.First().Move?.Name ?? "Unknown",
                        Description = group.First().Move?.Description ?? "No description available",
                        SkillLevel = group.First().Move?.SkillLevel ?? "Unknown",
                        TrainingLogCount = group.Count()
                    }).ToList();

                var totalGoalsAchieved = trainingGoals.Count(goal => goal.GoalDate <= DateTime.UtcNow);
                var totalMoves = trainingGoals
                    .SelectMany(goal => goal.UserTrainingGoalMoves)
                    .Select(gtm => gtm.Move)
                    .Distinct()
                    .Count();

                return new UserProgressDto
                {
                    TotalTrainingTime = totalTrainingTime,
                    TotalRoundsRolled = totalRoundsRolled,
                    TotalSubmissions = totalSubmissions,
                    TotalTaps = totalTaps,
                    WeeklyTrainingHours = weeklyTrainingHours,
                    AverageSessionLength = averageSessionLength,
                    FavoriteMoveThisMonth = favoriteMove,
                    TotalGoalsAchieved = totalGoalsAchieved,
                    TotalMoves = totalMoves,
                    MovesPerformed = movesPerformed
                };
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("An error occurred while fetching user progress.", ex);
            }
        }

        public async Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeAsync(string userId, DateTime fromDate, DateTime toDate)
        {
            return await GetDailyAggregatedFieldAsync(userId, fromDate, toDate, log => log.TrainingTime);
        }

        public async Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledAsync(string userId, DateTime fromDate, DateTime toDate)
        {
            return await GetDailyAggregatedFieldAsync(userId, fromDate, toDate, log => (double)log.RoundsRolled);
        }

        public async Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsAsync(string userId, DateTime fromDate, DateTime toDate)
        {
            return await GetDailyAggregatedFieldAsync(userId, fromDate, toDate, log => (double)log.Submissions);
        }

        public async Task<List<DailyUserProgressDto>> GetDailyTotalTapsAsync(string userId, DateTime fromDate, DateTime toDate)
        {
            return await GetDailyAggregatedFieldAsync(userId, fromDate, toDate, log => (double)log.Taps);
        }

        private async Task<List<DailyUserProgressDto>> GetDailyAggregatedFieldAsync(string userId, DateTime fromDate, DateTime toDate, Func<TrainingLog, double> selector)
        {
            var logs = await _context.TrainingLogs
                .Where(log => log.ApplicationUserId == userId && log.Date >= fromDate && log.Date <= toDate)
                .ToListAsync();

            var grouped = logs
                .GroupBy(log => log.Date.Date)
                .ToDictionary(
                    g => g.Key,
                    g => g.Sum(selector)
                );

            var result = new List<DailyUserProgressDto>();
            for (var date = fromDate.Date; date <= toDate.Date; date = date.AddDays(1))
            {
                result.Add(new DailyUserProgressDto
                {
                    Date = date,
                    Value = grouped.TryGetValue(date, out var value) ? value : 0
                });
            }
            return result;
        }

        // Convenience methods for common periods:
        public Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeLast7DaysAsync(string userId) =>
            GetDailyTotalTrainingTimeAsync(userId, DateTime.UtcNow.Date.AddDays(-6), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeLastMonthAsync(string userId) =>
            GetDailyTotalTrainingTimeAsync(userId, DateTime.UtcNow.Date.AddMonths(-1).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeLast6MonthsAsync(string userId) =>
            GetDailyTotalTrainingTimeAsync(userId, DateTime.UtcNow.Date.AddMonths(-6).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeLastYearAsync(string userId) =>
            GetDailyTotalTrainingTimeAsync(userId, DateTime.UtcNow.Date.AddYears(-1).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledLast7DaysAsync(string userId) =>
            GetDailyTotalRoundsRolledAsync(userId, DateTime.UtcNow.Date.AddDays(-6), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledLastMonthAsync(string userId) =>
            GetDailyTotalRoundsRolledAsync(userId, DateTime.UtcNow.Date.AddMonths(-1).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledLast6MonthsAsync(string userId) =>
            GetDailyTotalRoundsRolledAsync(userId, DateTime.UtcNow.Date.AddMonths(-6).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledLastYearAsync(string userId) =>
            GetDailyTotalRoundsRolledAsync(userId, DateTime.UtcNow.Date.AddYears(-1).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsLast7DaysAsync(string userId) =>
            GetDailyTotalSubmissionsAsync(userId, DateTime.UtcNow.Date.AddDays(-6), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsLastMonthAsync(string userId) =>
            GetDailyTotalSubmissionsAsync(userId, DateTime.UtcNow.Date.AddMonths(-1).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsLast6MonthsAsync(string userId) =>
            GetDailyTotalSubmissionsAsync(userId, DateTime.UtcNow.Date.AddMonths(-6).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsLastYearAsync(string userId) =>
            GetDailyTotalSubmissionsAsync(userId, DateTime.UtcNow.Date.AddYears(-1).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalTapsLast7DaysAsync(string userId) =>
            GetDailyTotalTapsAsync(userId, DateTime.UtcNow.Date.AddDays(-6), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalTapsLastMonthAsync(string userId) =>
            GetDailyTotalTapsAsync(userId, DateTime.UtcNow.Date.AddMonths(-1).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalTapsLast6MonthsAsync(string userId) =>
            GetDailyTotalTapsAsync(userId, DateTime.UtcNow.Date.AddMonths(-6).AddDays(1), DateTime.UtcNow.Date);

        public Task<List<DailyUserProgressDto>> GetDailyTotalTapsLastYearAsync(string userId) =>
            GetDailyTotalTapsAsync(userId, DateTime.UtcNow.Date.AddYears(-1).AddDays(1), DateTime.UtcNow.Date);
    }
}
