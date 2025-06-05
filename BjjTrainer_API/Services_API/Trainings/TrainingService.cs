using System.Collections.ObjectModel;
using BjjTrainer_API.Data;
using BjjTrainer_API.Models.DTO;
using BjjTrainer_API.Models.DTO.Moves;
using BjjTrainer_API.Models.DTO.TrainingLogDTOs;
using BjjTrainer_API.Models.DTO.UserDtos;
using BjjTrainer_API.Models.Joins;
using BjjTrainer_API.Models.Trainings;
using Microsoft.EntityFrameworkCore;

namespace BjjTrainer_API.Services_API.Trainings
{
    public class TrainingService
    {
        private readonly ApplicationDbContext _context;

        public TrainingService(ApplicationDbContext context) => _context = context;

        // ******************************** GET ALL TRAINING LOGS BY USER ********************************
        public async Task<List<TrainingLogDto>> GetTrainingLogsAsync(string userId)
        {
            return await _context.TrainingLogs
                .Where(log => log.ApplicationUserId == userId)
                .Include(log => log.TrainingLogMoves)
                .ThenInclude(tlm => tlm.Move)
                .Select(log => new TrainingLogDto
                {
                    Id = log.Id,
                    Date = log.Date,
                    TrainingTime = log.TrainingTime,
                    RoundsRolled = log.RoundsRolled,
                    Submissions = log.Submissions,
                    Taps = log.Taps,
                    Notes = log.Notes,
                    SelfAssessment = log.SelfAssessment,
                    MoveIds = log.TrainingLogMoves.Select(tlm => tlm.Move.Id).ToList()
                }).ToListAsync();
        }

        // ******************************** GET SINGLE TRAINING LOG BY ID ********************************
        public async Task<TrainingLogDto?> GetTrainingLogByIdAsync(int logId)
        {
            return await _context.TrainingLogs
                .Where(log => log.Id == logId)
                .Include(log => log.TrainingLogMoves)
                .ThenInclude(tlm => tlm.Move)
                .Select(log => new TrainingLogDto
                {
                    Id = log.Id,
                    Date = log.Date,
                    TrainingTime = log.TrainingTime,
                    RoundsRolled = log.RoundsRolled,
                    Submissions = log.Submissions,
                    Taps = log.Taps,
                    Notes = log.Notes,
                    SelfAssessment = log.SelfAssessment,
                    IsCoachLog = log.IsCoachLog,
                    Moves = log.TrainingLogMoves.Select(tlm => new LogMoveDto
                    {
                        Id = tlm.Move.Id,
                        Name = tlm.Move.Name
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        // ******************************** GET TRAINING LOG MOVES BY LOG ID ********************************
        public async Task<UpdateTrainingLogDto> GetTrainingLogMovesAsync(int logId)
        {
            var log = await _context.TrainingLogs
                .Where(t => t.Id == logId)
                .Include(t => t.TrainingLogMoves)
                .ThenInclude(tlm => tlm.Move)
                .Select(t => new
                {
                    t.Date,
                    t.TrainingTime,
                    t.RoundsRolled,
                    t.Submissions,
                    t.Taps,
                    t.Notes,
                    t.SelfAssessment,
                    t.IsCoachLog,
                    Moves = t.TrainingLogMoves.Select(tlm => new UpdateMoveDto
                    {
                        Id = tlm.Move.Id,
                        Name = tlm.Move.Name
                    }).ToList()
                })
                .FirstOrDefaultAsync();

            if (log == null)
            {
                throw new Exception($"Training log with ID {logId} not found.");
            }

            return new UpdateTrainingLogDto
            {
                Date = log.Date,
                TrainingTime = log.TrainingTime,
                RoundsRolled = log.RoundsRolled,
                Submissions = log.Submissions,
                Taps = log.Taps,
                Notes = log.Notes,
                SelfAssessment = log.SelfAssessment,
                IsCoachLog = log.IsCoachLog,
                Moves = new ObservableCollection<UpdateMoveDto>(log.Moves)
            };
        }

        public async Task<TrainingLogDto> GetTrainingLogByEventIdAsync(int eventId)
        {
            var log = await _context.TrainingLogs
                .Include(tl => tl.TrainingLogMoves)
                .Where(tl => tl.CalendarEventId == eventId)
                .FirstOrDefaultAsync();

            if (log == null)
                throw new Exception("Training log not found.");

            return new TrainingLogDto
            {
                Id = log.Id,
                Title = log.Title,
                Date = log.Date,
                ApplicationUserId = log.ApplicationUserId,
                IsCoachLog = log.IsCoachLog,
                CalendarEventId = log.CalendarEventId,
                MoveIds = log.TrainingLogMoves.Select(m => m.MoveId).ToList()
            };
        }

        // ******************************** GET MOVES BY IDS ********************************
        public async Task<List<MoveDto>> GetMovesByIdsAsync(List<int> moveIds)
        {
            if (moveIds == null || !moveIds.Any())
                throw new ArgumentException("Move IDs list cannot be empty.");

            return await _context.Moves
                .Where(m => moveIds.Contains(m.Id))
                .Select(m => new MoveDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description,
                    SkillLevel = m.SkillLevel
                }).ToListAsync();
        }

        // ******************************** UPDATE TRAINING LOG ********************************
        public async Task UpdateTrainingLogAsync(int logId, UpdateTrainingLogDto dto)
        {
            var log = await _context.TrainingLogs
                .Include(tl => tl.TrainingLogMoves)
                .FirstOrDefaultAsync(tl => tl.Id == logId);

            if (log == null)
                throw new Exception("Training log not found.");

            // Update training log properties
            log.Date = dto.Date;
            log.TrainingTime = dto.TrainingTime;
            log.RoundsRolled = dto.RoundsRolled;
            log.Submissions = dto.Submissions;
            log.Taps = dto.Taps;
            log.Notes = dto.Notes;
            log.SelfAssessment = dto.SelfAssessment;

            // Remove existing training log moves
            _context.TrainingLogMoves.RemoveRange(log.TrainingLogMoves);

            // Add new training log moves based on the provided Moves
            foreach (var move in dto.Moves)
            {
                _context.TrainingLogMoves.Add(new TrainingLogMove
                {
                    TrainingLogId = log.Id,
                    MoveId = move.Id // Use the Id property from UpdateMoveDto
                });
            }

            // Save changes to the database
            await _context.SaveChangesAsync();
        }

        // ******************************** ADD NEW TRAINING LOG ********************************
        public async Task AddTrainingLogAsync(string userId, TrainingLog trainingLog, List<int> moveIds)
        {
            var user = await _context.ApplicationUsers.FindAsync(userId);
            if (user == null)
                throw new Exception("User not found.");

            _context.TrainingLogs.Add(trainingLog);
            await _context.SaveChangesAsync();

            foreach (var moveId in moveIds)
            {
                _context.TrainingLogMoves.Add(new TrainingLogMove
                {
                    TrainingLogId = trainingLog.Id,
                    MoveId = moveId
                });
            }

            user.TotalTrainingTime += trainingLog.TrainingTime;
            user.TotalRoundsRolled += trainingLog.RoundsRolled;
            user.TotalSubmissions += trainingLog.Submissions;
            user.TotalTaps += trainingLog.Taps;

            await _context.SaveChangesAsync();
        }

        // ******************************** CREATE NEW TRAINING LOG ********************************
        public async Task AddTrainingLogAsync(CreateTrainingLogDto dto)
        {
            var user = await _context.ApplicationUsers.FindAsync(dto.ApplicationUserId);
            if (user == null)
                throw new Exception("User not found.");

            var trainingLog = new TrainingLog
            {
                ApplicationUserId = dto.ApplicationUserId,
                Date = dto.Date,
                TrainingTime = dto.TrainingTime,
                RoundsRolled = dto.RoundsRolled,
                Submissions = dto.Submissions,
                Taps = dto.Taps,
                Notes = dto.Notes ?? string.Empty,
                SelfAssessment = dto.SelfAssessment ?? string.Empty,
                IsCoachLog = dto.IsCoachLog,
                CalendarEventId = dto.CalendarEventId
            };

            _context.TrainingLogs.Add(trainingLog);
            await _context.SaveChangesAsync();

            if (dto.MoveIds != null)
            {
                foreach (var moveId in dto.MoveIds)
                {
                    _context.TrainingLogMoves.Add(new TrainingLogMove
                    {
                        TrainingLogId = trainingLog.Id,
                        MoveId = moveId
                    });
                }
            }

            // Update user stats as needed...

            await _context.SaveChangesAsync();
        }

        // ******************************** DELETE TRAINING LOG ********************************
        public async Task DeleteTrainingLogAsync(int logId)
        {
            var log = await _context.TrainingLogs
                .Include(tl => tl.TrainingLogMoves)
                .FirstOrDefaultAsync(tl => tl.Id == logId);

            if (log == null)
                throw new Exception("Training log not found.");

            _context.TrainingLogMoves.RemoveRange(log.TrainingLogMoves);
            _context.TrainingLogs.Remove(log);

            await _context.SaveChangesAsync();
        }

        // ******************************** DELETE TRAINING LOG MOVE ********************************
        public async Task RemoveTrainingLogMoveAsync(int logId, int moveId)
        {
            var logMove = await _context.TrainingLogMoves
                .FirstOrDefaultAsync(tlm => tlm.TrainingLogId == logId && tlm.MoveId == moveId);

            if (logMove != null)
            {
                _context.TrainingLogMoves.Remove(logMove);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Move not found for this training log.");
            }
        }

        // ******************************** GET USER PROGRESS ********************************
        public async Task<UserProgressDto> GetUserProgressAsync(string userId)
        {
            try
            {
                // Fetch user's training logs and related data
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

                // Return default progress if no logs or goals exist
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

                // Aggregate statistics
                var totalTrainingTime = userLogs.Sum(log => log.TrainingTime);
                var totalRoundsRolled = userLogs.Sum(log => log.RoundsRolled);
                var totalSubmissions = userLogs.Sum(log => log.Submissions);
                var totalTaps = userLogs.Sum(log => log.Taps);

                // Calculate weekly training hours
                var oneWeekAgo = DateTime.UtcNow.AddDays(-7);
                var weeklyTrainingHours = userLogs
                    .Where(log => log.Date >= oneWeekAgo)
                    .Sum(log => log.TrainingTime / 60);

                // Calculate average session length
                var averageSessionLength = userLogs.Any() ? userLogs.Average(log => log.TrainingTime) : 0;

                // Identify the most practiced move this month
                var currentMonth = DateTime.UtcNow.Month;
                var favoriteMove = userLogs
                    .Where(log => log.Date.Month == currentMonth)
                    .SelectMany(log => log.TrainingLogMoves)
                    .GroupBy(tlm => tlm.Move.Name)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault()?.Key ?? "No moves practiced";

                // Aggregate moves performed
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

                // Return user progress summary
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
