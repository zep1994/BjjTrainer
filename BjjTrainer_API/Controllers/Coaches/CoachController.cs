using BjjTrainer_API.Models.DTO.Calendars;
using BjjTrainer_API.Models.DTO.Coaches;
using BjjTrainer_API.Models.Users;
using BjjTrainer_API.Services_API.Coaches;
using BjjTrainer_API.Services_API.Trainings;
using BjjTrainer_API.Services_API.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BjjTrainer_API.Controllers.Coaches
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CoachController : ControllerBase
    {
        private readonly CoachService _coachService;
        private readonly TrainingService _trainingService;
        private readonly UserService _userService;

        public CoachController(CoachService coachService, TrainingService trainingService, UserService userService)
        {
            _coachService = coachService;
            _trainingService = trainingService;
            _userService = userService;
        }

        [HttpGet("events/full/{schoolId}")]
        public async Task<ActionResult<List<CoachEventDto>>> GetPastEventsWithLogs(int schoolId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            try
            {
                var events = await _coachService.GetPastEventsWithLogs(userId, schoolId);
                if (!events.Any())
                    return NotFound("No past events found.");

                return Ok(events);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("events/details/{eventId}")]
        public async Task<ActionResult<CoachEventDto>> GetEventDetails(int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            try
            {
                var eventDetails = await _coachService.GetEventDetailsAsync(eventId, userId);
                if (eventDetails == null)
                    return NotFound("Event not found.");

                return Ok(eventDetails);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("events/create")]
        public async Task<IActionResult> CreateEventWithMoves([FromBody] CreateEventDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            try
            {
                var createdEvent = await _coachService.CreateEventWithMovesAsync(userId, dto);
                return Ok(new { message = "Event created successfully.", eventId = createdEvent.Id });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("events/{eventId}")]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User is not authenticated.");

            try
            {
                await _coachService.DeleteEventAsync(eventId, userId);
                return Ok(new { Message = "Event deleted successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // List all users in the coach's school
        [HttpGet("school/users")]
        [Authorize]
        public async Task<IActionResult> GetSchoolUsers()
        {
            var coachId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var users = await _coachService.GetSchoolUsersAsync(coachId);
            return Ok(users.Select(u => new
            {
                u.Id,
                u.UserName,
                u.Email,
                u.Role,
                u.Belt,
                u.BeltStripes,
                u.PhoneNumber,
                u.ProfilePictureUrl,
                u.TrainingStartDate,
                u.LastLoginDate
            }));
        }

        // Add users to school by email
        [HttpPost("school/users/add")]
        [Authorize]
        public async Task<IActionResult> AddUsersByEmail([FromBody] List<string> emails)
        {
            var coachId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _coachService.AddUsersToSchoolByEmailAsync(coachId, emails);
            return Ok(result);
        }

        // Change user role (promote/demote)
        [HttpPut("school/users/{userId}/role")]
        [Authorize]
        public async Task<IActionResult> ChangeUserRole(string userId, [FromBody] string newRole)
        {
            var coachId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _coachService.ChangeUserRoleAsync(coachId, userId, newRole);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        // Remove user from school
        [HttpDelete("school/users/{userId}")]
        [Authorize]
        public async Task<IActionResult> RemoveUserFromSchool(string userId)
        {
            var coachId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _coachService.RemoveUserFromSchoolAsync(coachId, userId);
            if (!result.Success)
                return BadRequest(result.Message);
            return Ok(result.Message);
        }

        // Get user details
        [HttpGet("school/users/{userId}")]
        [Authorize]
        public async Task<IActionResult> GetUserDetails(string userId)
        {
            var coachId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _coachService.GetUserDetailsAsync(coachId, userId);
            if (user == null)
                return NotFound("User not found or not in your school.");
            return Ok(user);
        }
    }
}
