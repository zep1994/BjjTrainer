using BjjTrainer_API.Models.Calendars;
using BjjTrainer_API.Models.DTO.Calendars;
using BjjTrainer_API.Services_API.Calendars;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BjjTrainer_API.Controllers.Calendars
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController(CalendarService calendarService) : ControllerBase
    {
        private readonly CalendarService _calendarService = calendarService;

        // ******************************** GET EVENT BY ID ****************************************
        [HttpGet("{eventId}")]
        public async Task<IActionResult> GetEventById(int eventId)
        {
            if (eventId <= 0)
                return BadRequest(new { Message = "Invalid event ID." });

            try
            {
                var calendarEvent = await _calendarService.GetEventByIdAsync(eventId);

                return calendarEvent == null ? NotFound(new { Message = $"Event with ID {eventId} not found." }) : Ok(calendarEvent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching event {eventId}: {ex.Message}");
                return StatusCode(500, new { Message = "An error occurred while fetching the event." });
            }
        }

        // ******************************** CREATE EVENT (WITH LOG) ****************************************
        [HttpPost("events/create")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User not authorized." });

            try
            {
                var calendarEvent = await _calendarService.CreateEventAsync(userId, dto);
                return Ok(new
                {
                    message = "Event created successfully.",
                    eventId = calendarEvent.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // ******************************** USER EVENTS ****************************************
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserEvents(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest(new { Message = "User ID is required." });

            try
            {
                var events = await _calendarService.GetEventsForUserAsync(userId);

                return events == null || !events.Any() ? NotFound(new { Message = "No events found for this user." }) : Ok(events);
            }
            catch (Exception)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching events." });
            }
        }

        // ******************************** UPDATE EVENT ****************************************
        [HttpPut("events/{eventId}")]
        public async Task<IActionResult> UpdateEvent(int eventId, [FromBody] CalendarEvent model)
        {
            if (eventId <= 0)
                return BadRequest(new { Message = "Invalid event ID." });

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User is not authorized." });

            await _calendarService.UpdateEventAsync(eventId, model, userId);
            return Ok(new { Message = "Event updated successfully." });
        }

        // ******************************** DELETE EVENT ****************************************
        [HttpDelete("events/{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            if (eventId <= 0)
                return BadRequest(new { Message = "Invalid event ID." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            try
            {
                await _calendarService.DeleteEventAsync(eventId, userId);
                return Ok(new { Message = "Event deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // ******************************** STUDENT CHECK-IN AND LOG ****************************************
        [HttpPost("events/{eventId}/checkin")]
        public async Task<IActionResult> CheckInAndCreateStudentLog(int eventId)
        {
            if (eventId <= 0)
                return BadRequest(new { Message = "Invalid event ID." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User is not authorized." });

            try
            {
                var studentLog = await _calendarService.CheckInAndCreateStudentLogAsync(eventId, userId);
                return Ok(new
                {
                    Message = "Student log created successfully.",
                    LogId = studentLog.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // ******************************** GET USER SCHOOL ID ****************************************
        [HttpGet("user/schoolId")]
        public async Task<IActionResult> GetUserSchoolId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User is not authorized." });

            try
            {
                var schoolId = await _calendarService.GetUserSchoolIdAsync(userId);

                if (schoolId == null)
                    return NotFound(new { Message = "School ID not found for user." });

                return Ok(new { SchoolId = schoolId });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // ******************************** GET UPCOMING EVENTS COUNT ****************************************
        [HttpGet("school/{schoolId}/upcoming-events-count")]
        public async Task<IActionResult> GetUpcomingEventsCount(int schoolId)
        {
            var today = DateTime.UtcNow.Date;
            var todayMin = today.AddDays(-150);
            var weekFromNow = today.AddDays(7);

            var count = await _calendarService.GetUpcomingEventsCountAsync(schoolId, todayMin, weekFromNow);
            return Ok(new { count });
        }

        // ******************************** BOOK EVENT ****************************************
        [HttpPost("events/{eventId}/book")]
        public async Task<IActionResult> BookEvent(int eventId)
        {
            if (eventId <= 0)
                return BadRequest(new { Message = "Invalid event ID." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User is not authorized." });

            try
            {
                await _calendarService.BookEventAsync(eventId, userId);
                return Ok(new { Message = "Event booked successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // ******************************** UNBOOK EVENT ****************************************
        [HttpPost("events/{eventId}/unbook")]
        public async Task<IActionResult> UnbookEvent(int eventId)
        {
            if (eventId <= 0)
                return BadRequest(new { Message = "Invalid event ID." });

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { Message = "User is not authorized." });

            try
            {
                await _calendarService.UnbookEventAsync(eventId, userId);
                return Ok(new { Message = "Event unbooked successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // ******************************** GET EVENT STUDENT STATUSES ****************************************
        [HttpGet("events/{eventId}/students")]
        public async Task<IActionResult> GetEventStudentStatuses(
            int eventId, int pageNumber = 1, int pageSize = 20)
        {
            if (eventId <= 0)
                return BadRequest(new { Message = "Invalid event ID." });

            var coachUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(coachUserId))
                return Unauthorized(new { Message = "User is not authorized." });

            try
            {
                var students = await _calendarService.GetEventStudentStatusesAsync(eventId, coachUserId);
                return Ok(students);
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }
    }
}
