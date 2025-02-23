using BjjTrainer_API.Data;
using BjjTrainer_API.Models.Calendars;
using BjjTrainer_API.Models.DTO.Calendars;
using BjjTrainer_API.Models.DTO.Coaches;
using BjjTrainer_API.Models.DTO.Moves;
using BjjTrainer_API.Models.DTO.TrainingLogDTOs;
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
                }).ToList() : []
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
                }).ToList() : []
            };
        }


        public async Task<List<CalendarEventCheckIn>> GetCheckInsForEvent(int eventId)
        {
            return await _context.CalendarEventCheckIns
                .Where(c => c.CalendarEventId == eventId)
                .Include(c => c.User)
                .ToListAsync();
        }
    }
}
