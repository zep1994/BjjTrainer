using BjjTrainer_API.Data;
using BjjTrainer_API.Models.Calendars;
using BjjTrainer_API.Models.DTO.Calendars;
using BjjTrainer_API.Models.DTO.Coaches;
using BjjTrainer_API.Models.DTO.Moves;
using BjjTrainer_API.Models.DTO.TrainingLogDTOs;
using BjjTrainer_API.Models.Joins;
using BjjTrainer_API.Models.Trainings;
using BjjTrainer_API.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace BjjTrainer_API.Services_API.Coaches
{
    public class CoachService
    {
        private readonly ApplicationDbContext _context;

        public CoachService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CoachEventDto>> GetPastEventsWithLogs(string coachId, int schoolId)
        {
            // Ensure only coaches can access this data
            var coach = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);

            if (coach == null || coach.SchoolId != schoolId)
            {
                throw new UnauthorizedAccessException("You are not authorized to access these events.");
            }

            var events = await _context.CalendarEvents
                .Where(e => e.SchoolId == schoolId && e.EndDate < DateTime.UtcNow)
                .Include(e => e.TrainingLog)
                .ThenInclude(tl => tl.TrainingLogMoves)
                .ThenInclude(tlm => tlm.Move)
                .Include(e => e.CalendarEventCheckIns)
                .ThenInclude(ci => ci.User)
                .ToListAsync();

            var result = events.Select(ev => new CoachEventDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                StartDate = ev.StartDate,
                StartTime = ev.StartTime,
                EndDate = ev.EndDate,
                EndTime = ev.EndTime,
                IsAllDay = ev.IsAllDay,
                SchoolId = ev.SchoolId,
                CheckIns = ev.CalendarEventCheckIns.Select(c => new CheckInDto
                {
                    UserName = c.User.UserName,
                    CheckInTime = c.CheckInTime
                }).ToList(),
                Moves = ev.TrainingLog != null ? ev.TrainingLog.TrainingLogMoves.Select(tlm => new LogMoveDto
                {
                    Id = tlm.Move.Id,
                    Name = tlm.Move.Name
                }).ToList() : new List<LogMoveDto>()
            }).ToList();

            return result;
        }

        public async Task<CoachEventDto?> GetEventDetailsAsync(int eventId, string coachId)
        {
            // Ensure only coaches can access this data
            var coach = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);

            if (coach == null)
                throw new UnauthorizedAccessException("You are not authorized to access this event.");

            var ev = await _context.CalendarEvents
                .Where(e => e.Id == eventId)
                .Include(e => e.TrainingLog)
                .ThenInclude(tl => tl.TrainingLogMoves)
                .ThenInclude(tlm => tlm.Move)
                .Include(e => e.CalendarEventCheckIns)
                .ThenInclude(ci => ci.User)
                .FirstOrDefaultAsync();

            if (ev == null)
                return null;

            return new CoachEventDto
            {
                Id = ev.Id,
                Title = ev.Title,
                Description = ev.Description,
                StartDate = ev.StartDate,
                StartTime = ev.StartTime,
                EndDate = ev.EndDate,
                EndTime = ev.EndTime,
                IsAllDay = ev.IsAllDay,
                SchoolId = ev.SchoolId,
                CheckIns = ev.CalendarEventCheckIns.Select(c => new CheckInDto
                {
                    UserName = c.User.UserName,
                    CheckInTime = c.CheckInTime
                }).ToList(),
                Moves = ev.TrainingLog != null ? ev.TrainingLog.TrainingLogMoves.Select(tlm => new LogMoveDto
                {
                    Id = tlm.Move.Id,
                    Name = tlm.Move.Name
                }).ToList() : new List<LogMoveDto>()
            };
        }

        public async Task<List<CalendarEventCheckIn>> GetCheckInsForEvent(int eventId)
        {
            return await _context.CalendarEventCheckIns
                .Where(c => c.CalendarEventId == eventId)
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<CalendarEvent> CreateEventWithMovesAsync(string coachId, CreateEventDto dto)
        {
            var coach = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach)
                ?? throw new UnauthorizedAccessException("Only coaches can create events.");

            if (coach.SchoolId == null)
                throw new Exception("Coach must be assigned to a school to create events.");

            var calendarEvent = new CalendarEvent
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                StartTime = dto.StartTime,
                EndDate = dto.EndDate,
                EndTime = dto.EndTime,
                IsAllDay = dto.IsAllDay,
                SchoolId = coach.SchoolId // Always set to coach's school
            };

            _context.CalendarEvents.Add(calendarEvent);
            await _context.SaveChangesAsync();

            _context.CalendarEventUsers.Add(new CalendarEventUser
            {
                CalendarEventId = calendarEvent.Id,
                UserId = coachId
            });

            if (dto.IncludeTrainingLog)
            {
                var trainingLog = new TrainingLog
                {
                    Title = $"{dto.Title} - Class Log",
                    ApplicationUserId = coachId,
                    Date = dto.StartDate,
                    CalendarEventId = calendarEvent.Id,
                    IsCoachLog = true
                };
                _context.TrainingLogs.Add(trainingLog);
                await _context.SaveChangesAsync();

                if (dto.MoveIds != null && dto.MoveIds.Any())
                {
                    foreach (var moveId in dto.MoveIds)
                    {
                        _context.TrainingLogMoves.Add(new TrainingLogMove
                        {
                            TrainingLogId = trainingLog.Id,
                            MoveId = moveId,
                            IsCoachSelected = true
                        });
                    }
                    await _context.SaveChangesAsync();
                }
            }

            await _context.SaveChangesAsync();
            return calendarEvent;
        }

        public async Task DeleteEventAsync(int eventId, string coachId)
        {
            var coach = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach)
                ?? throw new UnauthorizedAccessException("Only coaches can delete events.");

            var calendarEvent = await _context.CalendarEvents
                .Include(e => e.CalendarEventUsers)
                .FirstOrDefaultAsync(e => e.Id == eventId && e.SchoolId == coach.SchoolId);

            if (calendarEvent == null)
                throw new Exception("Event not found or not authorized.");

            _context.CalendarEventUsers.RemoveRange(calendarEvent.CalendarEventUsers);
            _context.CalendarEvents.Remove(calendarEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetSchoolUsersAsync(string coachId)
        {
            var coach = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);
            if (coach == null || coach.SchoolId == null)
                return new List<ApplicationUser>();
            return await _context.ApplicationUsers.Where(u => u.SchoolId == coach.SchoolId).ToListAsync();
        }

        public async Task<string> AddUsersToSchoolByEmailAsync(string coachId, List<string> emails)
        {
            var coach = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);
            if (coach == null || coach.SchoolId == null)
                return "Coach is not assigned to a school.";

            var results = new List<string>();
            foreach (var email in emails)
            {
                var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    results.Add($"User with email {email} not found.");
                    continue;
                }
                if (user.SchoolId == coach.SchoolId)
                {
                    results.Add($"User {email} is already a member of this school.");
                    continue;
                }
                user.SchoolId = coach.SchoolId;
                _context.ApplicationUsers.Update(user);
                results.Add($"User {email} has been added to the school.");
            }
            await _context.SaveChangesAsync();
            return string.Join("\n", results);
        }

        public async Task<(bool Success, string Message)> ChangeUserRoleAsync(string coachId, string userId, string newRole)
        {
            var coach = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (coach == null || user == null || coach.SchoolId != user.SchoolId)
                return (false, "Invalid coach or user, or not in the same school.");

            if (!Enum.TryParse<UserRole>(newRole, out var parsedRole))
                return (false, "Invalid role.");

            user.Role = parsedRole;
            _context.ApplicationUsers.Update(user);
            await _context.SaveChangesAsync();
            return (true, $"User {user.UserName} role updated to {newRole}.");
        }

        public async Task<(bool Success, string Message)> RemoveUserFromSchoolAsync(string coachId, string userId)
        {
            var coach = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (coach == null || user == null || coach.SchoolId != user.SchoolId)
                return (false, "Invalid coach or user, or not in the same school.");

            user.SchoolId = null;
            _context.ApplicationUsers.Update(user);
            await _context.SaveChangesAsync();
            return (true, $"User {user.UserName} removed from school.");
        }

        public async Task<object?> GetUserDetailsAsync(string coachId, string userId)
        {
            var coach = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (coach == null || user == null || coach.SchoolId != user.SchoolId)
                return null;

            return new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Role,
                user.Belt,
                user.BeltStripes,
                user.PhoneNumber,
                user.ProfilePictureUrl,
                user.TrainingStartDate,
                user.LastLoginDate,
                user.PreferredTrainingStyle
            };
        }
    }
}
