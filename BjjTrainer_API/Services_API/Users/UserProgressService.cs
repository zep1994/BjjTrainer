using BjjTrainer_API.Data;
using BjjTrainer_API.Models.DTO;
using BjjTrainer_API.Models.DTO.Moves;
using BjjTrainer_API.Models.DTO.UserDtos;
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
                    .GroupBy(tlm => tlm.Move.Name)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key ?? "No moves practiced";

                var movesPerformed = userLogs
                    .SelectMany(log => log.TrainingLogMoves)
                    .GroupBy(tlm => tlm.Move.Id)
                    .Select(group => new MoveDto
                    {
                        Id = group.Key,
                        Name = group.First().Move.Name,
                        Description = group.First().Move.Description,
                        SkillLevel = group.First().Move.SkillLevel,
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
    }
}
