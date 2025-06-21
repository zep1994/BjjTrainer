using BjjTrainer_API.Data;
using BjjTrainer_API.Models.Calendars;
using BjjTrainer_API.Models.DTO.Calendars;
using BjjTrainer_API.Models.DTO.Coaches;
using BjjTrainer_API.Models.DTO.Moves;
using BjjTrainer_API.Models.DTO.Students;
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
                CheckIns = ev.CalendarEventCheckIns?.Select(c => new CheckInDto
                {
                    UserName = c.User?.UserName ?? "Unknown",
                    CheckInTime = c.CheckInTime
                }).ToList() ?? [],
                Moves = ev.TrainingLog?.TrainingLogMoves?.Select(tlm => new LogMoveDto
                {
                    Id = tlm.Move?.Id ?? 0,
                    Name = tlm.Move?.Name ?? "Unknown"
                }).ToList() ?? []
            }).ToList();

            return result;
        }

        public async Task<CoachEventDto?> GetEventDetailsAsync(int eventId, string coachId)
        {
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

            return ev == null
                ? null
                : new CoachEventDto
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
                CheckIns = ev.CalendarEventCheckIns?.Select(c => new CheckInDto
                {
                    UserName = c.User?.UserName ?? "Unknown",
                    CheckInTime = c.CheckInTime
                }).ToList() ?? [],
                Moves = ev.TrainingLog?.TrainingLogMoves?.Select(tlm => new LogMoveDto
                {
                    Id = tlm.Move?.Id ?? 0,
                    Name = tlm.Move?.Name ?? "Unknown"
                }).ToList() ?? []
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

            if (dto.Title == null)
                throw new ArgumentNullException(nameof(dto.Title), "Event title cannot be null.");

            var calendarEvent = new CalendarEvent
            {
                Title = dto.Title,
                Description = dto.Description,
                StartDate = dto.StartDate,
                StartTime = dto.StartTime,
                EndDate = dto.EndDate,
                EndTime = dto.EndTime,
                IsAllDay = dto.IsAllDay,
                SchoolId = coach.SchoolId,
                InstructorId = dto.InstructorId
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

                if (dto.MoveIds?.Count > 0)
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

            return calendarEvent;
        }

        public async Task DeleteEventAsync(int eventId, string coachId)
        {
            var coach = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach)
                ?? throw new UnauthorizedAccessException("Only coaches can delete events.");

            var calendarEvent = await _context.CalendarEvents
                .Include(e => e.CalendarEventUsers)
                .FirstOrDefaultAsync(e => e.Id == eventId && e.SchoolId == coach.SchoolId)
                ?? throw new Exception("Event not found or not authorized.");

            _context.CalendarEventUsers.RemoveRange(calendarEvent.CalendarEventUsers);
            _context.CalendarEvents.Remove(calendarEvent);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationUser>> GetSchoolUsersAsync(string coachId)
        {
            var coach = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);
            return coach == null || coach.SchoolId == null
                ? []
                : await _context.ApplicationUsers.Where(u => u.SchoolId == coach.SchoolId).ToListAsync();
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
            return coach == null || user == null || coach.SchoolId != user.SchoolId
                ? null
                : (object)(new
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
            });
        }

        public async Task<List<SchoolStudentDto>> GetSchoolStudentsWithAttendanceAsync(string coachId)
        {
            var coach = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);

            if (coach == null || coach.SchoolId == null)
                throw new UnauthorizedAccessException("Coach is not assigned to a school.");

            // Get all students in the coach's school
            var students = await _context.ApplicationUsers
                .Where(u => u.SchoolId == coach.SchoolId && u.Role == UserRole.Student)
                .ToListAsync();

            // Get all check-ins for students in this school
            var studentIds = students.Select(s => s.Id).ToList();
            var checkInCounts = await _context.CalendarEventCheckIns
                .Where(ci => ci.UserId != null && studentIds.Contains(ci.UserId))
                .GroupBy(ci => ci.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.UserId!, g => g.Count);

            // Build DTOs
            var result = students.Select(s => new SchoolStudentDto
            {
                UserId = s.Id,
                UserName = s.UserName ?? "",
                Email = s.Email ?? "",
                ProfilePictureUrl = s.ProfilePictureUrl ?? "",
                Belt = s.Belt,
                BeltStripes = s.BeltStripes,
                TrainingStartDate = s.TrainingStartDate,
                LastLoginDate = s.LastLoginDate,
                EventsAttended = checkInCounts.TryGetValue(s.Id, out var count) ? count : 0
            }).ToList();

            return result;
        }

        public async Task<SchoolAttendanceSummaryDto> GetSchoolAttendanceSummaryAsync(string coachId)
        {
            var coach = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);

            if (coach == null || coach.SchoolId == null)
                throw new UnauthorizedAccessException("Coach is not assigned to a school.");

            var schoolId = coach.SchoolId.Value;

            var totalStudents = await _context.ApplicationUsers
                .CountAsync(u => u.SchoolId == schoolId && u.Role == UserRole.Student);

            var totalEvents = await _context.CalendarEvents
                .CountAsync(e => e.SchoolId == schoolId);

            var totalCheckIns = await _context.CalendarEventCheckIns
                .CountAsync(ci => ci.User != null && ci.User.SchoolId == schoolId);

            double avgAttendancePerEvent = totalEvents > 0 ? (double)totalCheckIns / totalEvents : 0;
            double avgAttendancePerStudent = totalStudents > 0 ? (double)totalCheckIns / totalStudents : 0;

            return new SchoolAttendanceSummaryDto
            {
                TotalStudents = totalStudents,
                TotalEvents = totalEvents,
                TotalCheckIns = totalCheckIns,
                AverageAttendancePerEvent = Math.Round(avgAttendancePerEvent, 2),
                AverageAttendancePerStudent = Math.Round(avgAttendancePerStudent, 2)
            };
        }

        public async Task<SchoolAttendanceSummaryDto> GetStudentAttendanceSummaryAsync(string coachId, string studentId)
        {
            var coach = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);

            var student = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == studentId && u.Role == UserRole.Student);

            if (coach == null || student == null || coach.SchoolId == null || coach.SchoolId != student.SchoolId)
                throw new UnauthorizedAccessException("Coach and student must be in the same school.");

            var totalEventsAttended = await _context.CalendarEventCheckIns
                .CountAsync(ci => ci.UserId == studentId && ci.User != null && ci.User.SchoolId == coach.SchoolId);

            var records = await _context.CalendarEventCheckIns
                .Where(ci => ci.UserId == studentId && ci.User != null && ci.User.SchoolId == coach.SchoolId)
                .Include(ci => ci.CalendarEvent)
                .OrderByDescending(ci => ci.CheckInTime)
                .Select(ci => new SchoolAttendanceRecordDto
                {
                    EventId = ci.CalendarEventId,
                    EventTitle = ci.CalendarEvent != null ? ci.CalendarEvent.Title : "Unknown",
                    EventDate = ci.CalendarEvent != null ? ci.CalendarEvent.StartDate : null,
                    CheckInTime = ci.CheckInTime
                })
                .ToListAsync();

            return new SchoolAttendanceSummaryDto
            {
                TotalEventsAttended = totalEventsAttended,
                Records = records
            };
        }

        public async Task<List<SchoolAttendanceRecordDto>> GetStudentAttendanceRecordsAsync(string coachId, string studentId)
        {
            var coach = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == coachId && u.Role == UserRole.Coach);

            var student = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == studentId && u.Role == UserRole.Student);

            if (coach == null || student == null || coach.SchoolId == null || coach.SchoolId != student.SchoolId)
                throw new UnauthorizedAccessException("Coach and student must be in the same school.");

            var records = await _context.CalendarEventCheckIns
                .Where(ci => ci.UserId == studentId && ci.User != null && ci.User.SchoolId == coach.SchoolId)
                .Include(ci => ci.CalendarEvent)
                .OrderByDescending(ci => ci.CheckInTime)
                .Select(ci => new SchoolAttendanceRecordDto
                {
                    EventId = ci.CalendarEventId,
                    EventTitle = ci.CalendarEvent != null ? ci.CalendarEvent.Title : "Unknown",
                    EventDate = ci.CalendarEvent != null ? ci.CalendarEvent.StartDate : null,
                    CheckInTime = ci.CheckInTime
                })
                .ToListAsync();

            return records;
        }
    }
}
