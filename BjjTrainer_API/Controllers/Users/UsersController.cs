using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Services_API.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BjjTrainer_API.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        // POST: api/users/refresh-token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            var tokens = await _userService.RefreshTokensAsync(refreshToken);

            if (tokens == null)
                return Unauthorized(new { message = "Invalid or expired refresh token." });

            return Ok(tokens);
        }

        // Get all favorite lessons for a specific user
        [HttpGet("{userId}/favorites")]
        public async Task<ActionResult<List<Lesson>>> GetUserFavorites(string userId)
        {
            var favorites = await _userService.GetUserFavoritesAsync(userId);
            return Ok(favorites);
        }

        // Add a lesson to the user's favorites
        [HttpPost("{userId}/favorites/{lessonId}")]
        public async Task<IActionResult> AddLessonToFavorites(string userId, int lessonId)
        {
            var success = await _userService.AddLessonToFavoritesAsync(userId, lessonId);
            if (success)
            {
                return Ok("Lesson added to favorites.");
            }
            return BadRequest("Failed to add lesson to favorites.");
        }

        // Remove a lesson from the user's favorites
        [HttpDelete("{userId}/favorites/{lessonId}")]
        public async Task<IActionResult> RemoveLessonFromFavorites(string userId, int lessonId)
        {
            var success = await _userService.RemoveLessonFromFavoritesAsync(userId, lessonId);
            if (success)
            {
                return Ok("Lesson removed from favorites.");
            }
            return BadRequest("Failed to remove lesson from favorites.");
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            var result = new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Role,
                user.Belt,
                user.BeltStripes,
                user.TotalTrainingTime
            };

            return Ok(result);
        }


        [HttpPost("{schoolId}/add-users")]
        [Authorize]
        public async Task<IActionResult> AddUsersToSchool(int schoolId, [FromBody] List<string> emails)
        {
            var coachId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(coachId))
            {
                return Unauthorized("User not logged in.");
            }

            var result = await _userService.AddUsersToSchoolAsync(coachId, schoolId, emails);
            return Ok(result);
        }


        [HttpPut("{userId}/school/{schoolId}")]
        public async Task<IActionResult> UpdateUserSchool(string userId, int schoolId)
        {
            var success = await _userService.UpdateUserSchoolAsync(userId, schoolId);
            if (success)
            {
                // Enroll the user in future school events
                await _userService.EnrollUserInSchoolEvents(userId, schoolId);
                return Ok("User's school updated and enrolled in future events.");
            }

            return BadRequest("Failed to update user's school.");
        }


        [HttpGet("school/{schoolId}/users")]
        public async Task<IActionResult> GetUsersBySchool(int schoolId)
        {
            var users = await _userService.GetUsersBySchoolAsync(schoolId);
            return Ok(users);
        }

        [HttpGet("school/{schoolId}/students")]
        public async Task<IActionResult> GetStudentsBySchool(int schoolId)
        {
            var students = await _userService.GetUsersBySchoolAsync(schoolId);
            return Ok(students);
        }
    }
}
