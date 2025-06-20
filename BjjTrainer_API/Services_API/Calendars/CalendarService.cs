﻿using BjjTrainer_API.Data;
using BjjTrainer_API.Models.Calendars;
using BjjTrainer_API.Models.DTO.Calendars;
using BjjTrainer_API.Models.Joins;
using BjjTrainer_API.Models.Trainings;
using BjjTrainer_API.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace BjjTrainer_API.Services_API.Calendars
{
    public class CalendarService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        // ******************************** GET EVENT BY ID ****************************************
        public async Task<CalendarEventDto> GetEventByIdAsync(int eventId)
        {
            var calendarEvent = await _context.CalendarEvents
                .Where(e => e.Id == eventId)
                .Select(e => new CalendarEventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    StartDate = e.StartDate,
                    StartTime = e.StartTime,
                    EndDate = e.EndDate,
                    EndTime = e.EndTime,
                    TrainingLogId = e.TrainingLogId,
                    InstructorId = e.InstructorId
                })
                .FirstOrDefaultAsync();

            return calendarEvent ?? throw new Exception("Event not found.");
        }

        // ******************************** CREATE EVENT ****************************************
        public async Task<CalendarEvent> CreateEventAsync(string userId, CreateEventDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title)) 
                throw new ArgumentNullException(nameof(dto), "Event title cannot be null.");

            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new Exception("User not found.");

            var calendarEvent = new CalendarEvent
            {
                Title = dto.Title,
                StartDate = dto.StartDate,
                StartTime = dto.StartTime,
                EndDate = dto.EndDate,
                EndTime = dto.EndTime
            };

            _context.CalendarEvents.Add(calendarEvent);
            await _context.SaveChangesAsync();

            var eventUser = new CalendarEventUser
            {
                CalendarEventId = calendarEvent.Id,
                UserId = userId
            };
            _context.CalendarEventUsers.Add(eventUser);
            await _context.SaveChangesAsync();

            if (dto.IncludeTrainingLog)
            {
                var trainingLog = new TrainingLog
                {
                    Title = $"{dto.Title} - Class Log",
                    ApplicationUserId = userId,
                    Date = dto.StartDate,
                    CalendarEventId = calendarEvent.Id,
                    IsCoachLog = user.Role == UserRole.Coach
                };

                _context.TrainingLogs.Add(trainingLog);
                await _context.SaveChangesAsync();

                if (dto.MoveIds.Any())
                {
                    foreach (var moveId in dto.MoveIds)
                    {
                        _context.TrainingLogMoves.Add(new TrainingLogMove
                        {
                            TrainingLogId = trainingLog.Id,
                            MoveId = moveId,
                            IsCoachSelected = user.Role == UserRole.Coach
                        });
                    }
                }
                await _context.SaveChangesAsync();
            }

            return calendarEvent;
        }

        public async Task UpdateEventAsync(int eventId, CalendarEvent model, string userId)
        {
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == userId) ?? throw new Exception("User not found.");

            var calendarEvent = await _context.CalendarEvents
                .FirstOrDefaultAsync(e => e.Id == eventId) ?? throw new Exception("Event not found.");

            if (user.Role == UserRole.Student && calendarEvent.SchoolId != null)
            {
                throw new Exception("Students can only update their personal events.");
            }

            if (user.Role == UserRole.Coach && calendarEvent.SchoolId != user.SchoolId)
            {
                throw new Exception("You can only update events for your school.");
            }

            calendarEvent.Title = model.Title;
            calendarEvent.Description = model.Description;
            calendarEvent.StartDate = model.StartDate;
            calendarEvent.StartTime = model.StartTime != null && TimeSpan.TryParse(model.StartTime.ToString(), out var st) ? st : null;
            calendarEvent.EndDate = model.EndDate;
            calendarEvent.EndTime = model.EndTime != null && TimeSpan.TryParse(model.EndTime.ToString(), out var et) ? et : null;
            calendarEvent.IsAllDay = model.IsAllDay;
            calendarEvent.SchoolId = model.SchoolId;
            calendarEvent.InstructorId = model.InstructorId;
            await _context.SaveChangesAsync();
        }

        // ******************************** CHECK-IN AND LOG CREATION ****************************************
        public async Task<TrainingLog> CheckInAndCreateStudentLogAsync(int eventId, string userId)
        {
            var calendarEvent = await _context.CalendarEvents
                .Include(e => e.TrainingLog)
                .ThenInclude(tl => tl.TrainingLogMoves)
                .FirstOrDefaultAsync(e => e.Id == eventId)
                ?? throw new Exception("Event not found.");

            if (calendarEvent.EndDate == null || calendarEvent.EndDate > DateTime.UtcNow.Date)
                throw new Exception("Check-in is only allowed after the event has ended.");

            var alreadyCheckedIn = await _context.CalendarEventCheckIns
                .AnyAsync(c => c.CalendarEventId == eventId && c.UserId == userId);

            if (alreadyCheckedIn)
                throw new Exception("You have already checked into this event.");

            // Perform check-in
            var checkIn = new CalendarEventCheckIn
            {
                CalendarEventId = eventId,
                UserId = userId,
                CheckInTime = DateTime.UtcNow
            };
            _context.CalendarEventCheckIns.Add(checkIn);

            // Create the student's personal training log
            var studentLog = new TrainingLog
            {
                Title = $"{calendarEvent.Title} - Personal Log",
                ApplicationUserId = userId,
                Date = calendarEvent.StartDate ?? DateTime.UtcNow,
                CalendarEventId = eventId,
                IsCoachLog = false,
                IsShared = false
            };

            _context.TrainingLogs.Add(studentLog);
            await _context.SaveChangesAsync();

            // Clone moves from the coach's training log (if they exist)
            if (calendarEvent.TrainingLog?.TrainingLogMoves != null)
            {
                foreach (var move in calendarEvent.TrainingLog.TrainingLogMoves)
                {
                    _context.TrainingLogMoves.Add(new TrainingLogMove
                    {
                        TrainingLogId = studentLog.Id,
                        MoveId = move.MoveId,
                        IsCoachSelected = true  // Preserve coach selection
                    });
                }
            }

            await _context.SaveChangesAsync();
            return studentLog;
        }

        public async Task<int?> GetUserSchoolIdAsync(string userId)
        {
            return await _context.ApplicationUsers
                .Where(u => u.Id == userId)
                .Select(u => u.SchoolId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<CalendarEvent>> GetEventsForUserAsync(string userId)
        {
            var user = await _context.ApplicationUsers.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new Exception("User not found.");

            var userSchoolId = user.SchoolId;

            var events = await _context.CalendarEvents
                .Where(e =>
                    e.CalendarEventUsers.Any(eu => eu.UserId == userId) ||
                    (userSchoolId != null && e.SchoolId == userSchoolId)
                )
                .ToListAsync();

            return events;
        }

        public async Task CheckInEventAsync(int eventId, string userId)
        {
            var eventUser = await _context.CalendarEventUsers
                .FirstOrDefaultAsync(eu => eu.CalendarEventId == eventId && eu.UserId == userId);

            if (eventUser == null)
                throw new Exception("You must join the event before checking in.");

            if (eventUser.IsCheckedIn)
                throw new Exception("You have already checked in.");

            eventUser.IsCheckedIn = true;

            _context.CalendarEventCheckIns.Add(new CalendarEventCheckIn
            {
                CalendarEventId = eventId,
                UserId = userId,
                CheckInTime = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
        }

        // ******************************** DELETE EVENT ****************************************
        public async Task DeleteEventAsync(int eventId, string userId)
        {
            var user = await _context.ApplicationUsers.FindAsync(userId);
            var calendarEvent = await _context.CalendarEvents
                .Include(e => e.CalendarEventUsers)
                .FirstOrDefaultAsync(e => e.Id == eventId);

            if (calendarEvent == null)
                throw new Exception("Event not found.");

            if (calendarEvent.EndDate < DateTime.UtcNow)
                throw new Exception("Cannot delete past events.");

            if (user != null && user.Role == UserRole.Coach)
            {
                _context.CalendarEventUsers.RemoveRange(calendarEvent.CalendarEventUsers);
                _context.CalendarEvents.Remove(calendarEvent);
            }
            else
            {
                var userEvent = calendarEvent.CalendarEventUsers
                    .FirstOrDefault(ceu => ceu.UserId == userId && ceu.User != null && ceu.User.Role == UserRole.Student);

                if (userEvent != null)
                {
                    _context.CalendarEventUsers.Remove(userEvent);

                    if (!calendarEvent.CalendarEventUsers.Any())
                    {
                        _context.CalendarEvents.Remove(calendarEvent);
                    }
                }
                else
                {
                    throw new Exception("You can only delete your personal events.");
                }
                if (userEvent != null)
                {
                    _context.CalendarEventUsers.Remove(userEvent);

                    if (!calendarEvent.CalendarEventUsers.Any())
                    {
                        _context.CalendarEvents.Remove(calendarEvent);
                    }
                }
                else
                {
                    throw new Exception("You can only delete your personal events.");
                }
            }

            await _context.SaveChangesAsync();
        }

        // ******************************** GET UPCOMING EVENTS COUNT ****************************************
        public async Task<int> GetUpcomingEventsCountAsync(int schoolId, DateTime start, DateTime end)
        {
            return await _context.CalendarEvents
                .Where(e => e.SchoolId == schoolId && e.StartDate >= start && e.StartDate < end)
                .CountAsync();
        }

        // ******************************** BOOK EVENT ****************************************
        public async Task BookEventAsync(int eventId, string userId)
        {
            var user = await _context.ApplicationUsers
                .FirstOrDefaultAsync(u => u.Id == userId)
                ?? throw new Exception("User not found.");

            var calendarEvent = await _context.CalendarEvents
                .FirstOrDefaultAsync(e => e.Id == eventId)
                ?? throw new Exception("Event not found.");

            // Only allow booking for upcoming events
            if (calendarEvent.StartDate == null || calendarEvent.StartDate < DateTime.UtcNow.Date)
                throw new Exception("You can only book upcoming events.");

            // Only allow booking for user's school
            if (calendarEvent.SchoolId == null || user.SchoolId == null || calendarEvent.SchoolId != user.SchoolId)
                throw new Exception("You can only book events for your school.");

            // Find existing CalendarEventUser record
            var eventUser = await _context.CalendarEventUsers
                .FirstOrDefaultAsync(eu => eu.CalendarEventId == eventId && eu.UserId == userId);

            if (eventUser != null)
            {
                if (eventUser.IsBooked)
                    throw new Exception("You have already booked this event.");

                eventUser.IsBooked = true;
            }
            else
            {
                eventUser = new CalendarEventUser
                {
                    CalendarEventId = eventId,
                    UserId = userId,
                    IsBooked = true,
                    IsCheckedIn = false
                };
                _context.CalendarEventUsers.Add(eventUser);
            }

            await _context.SaveChangesAsync();
        }

        // ******************************** UNBOOK EVENT ****************************************
        public async Task UnbookEventAsync(int eventId, string userId)
        {
            var eventUser = await _context.CalendarEventUsers
                .FirstOrDefaultAsync(eu => eu.CalendarEventId == eventId && eu.UserId == userId);

            if (eventUser == null || !eventUser.IsBooked)
                throw new Exception("You have not booked this event.");

            eventUser.IsBooked = false;
            await _context.SaveChangesAsync();
        }

        // ******************************** GET EVENT STUDENT STATUSES ****************************************
        public async Task<List<CalendarEventStudentStatusDto>> GetEventStudentStatusesAsync(
            int eventId, string coachUserId)
        {
            // Ensure the requesting user is a coach
            var coach = await _context.ApplicationUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == coachUserId && u.Role == UserRole.Coach);
            if (coach == null)
                throw new Exception("Only coaches can view this information.");

            // Load the event and check school
            var calendarEvent = await _context.CalendarEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == eventId);
            if (calendarEvent == null)
                throw new Exception("Event not found.");
            if (calendarEvent.SchoolId != coach.SchoolId)
                throw new Exception("You can only view events for your school.");

            // Query all students who have booked or checked in for this event
            var students = await _context.CalendarEventUsers
                .AsNoTracking()
                .Where(eu => eu.CalendarEventId == eventId && (eu.IsBooked || eu.IsCheckedIn))
                .Include(eu => eu.User)
                .OrderBy(eu => eu.User.UserName)
                .Select(eu => new CalendarEventStudentStatusDto
                {
                    UserId = eu.UserId ?? "",
                    UserName = eu.User != null ? eu.User.UserName : null,
                    ProfilePictureUrl = eu.User != null ? eu.User.ProfilePictureUrl : null,
                    IsBooked = eu.IsBooked,
                    IsCheckedIn = eu.IsCheckedIn
                })
                .ToListAsync();

            return students;
        }

        public async Task<(List<CalendarEventStudentStatusDto> Students, int TotalCount)> GetEventStudentStatusesAsync(
            int eventId, string coachUserId, int pageNumber, int pageSize)
        {
            // Ensure the requesting user is a coach
            var coach = await _context.ApplicationUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == coachUserId && u.Role == UserRole.Coach);
            if (coach == null)
                throw new Exception("Only coaches can view this information.");

            // Load the event and check school
            var calendarEvent = await _context.CalendarEvents
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == eventId);
            if (calendarEvent == null)
                throw new Exception("Event not found.");
            if (calendarEvent.SchoolId != coach.SchoolId)
                throw new Exception("You can only view events for your school.");

            // Query all students who have booked or checked in for this event
            var query = _context.CalendarEventUsers
                .AsNoTracking()
                .Where(eu => eu.CalendarEventId == eventId)
                .Include(eu => eu.User)
                .Select(eu => new CalendarEventStudentStatusDto
                {
                    UserId = eu.UserId ?? "",
                    UserName = eu.User != null ? eu.User.UserName : null,
                    ProfilePictureUrl = eu.User != null ? eu.User.ProfilePictureUrl : null,
                    IsBooked = eu.IsBooked,
                    IsCheckedIn = eu.IsCheckedIn
                });

            var totalCount = await query.CountAsync();
            var students = await query
                .OrderBy(eu => eu.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (students, totalCount);
        }
    }
}
