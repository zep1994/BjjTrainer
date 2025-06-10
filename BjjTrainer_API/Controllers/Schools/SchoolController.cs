using BjjTrainer_API.Models.DTO.UserDtos;
using BjjTrainer_API.Models.Users;
using BjjTrainer_API.Services_API.Schools;
using BjjTrainer_API.Services_API.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BjjTrainer_API.Controllers.Schools
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController : ControllerBase
    {
        public SchoolController(
            SchoolService schoolService,
            UserService userService,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _schoolService = schoolService;
            _userService = userService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly SchoolService _schoolService;
        private readonly UserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [HttpGet("coach/{coachId}")]
        public async Task<IActionResult> GetSchoolByCoach(string coachId)
        {
            var school = await _schoolService.GetSchoolByCoachIdAsync(coachId);

            if (school == null)
            {
                return NotFound("School not found or coach is not assigned to a school.");
            }

            return Ok(school);
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateSchool([FromBody] SchoolCreateRequest request)
        {
            // Get the currently logged-in user (the coach)
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            // Validate the request to ensure no null values are passed
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Address) || string.IsNullOrEmpty(request.Phone))
            {
                return BadRequest("All fields (Name, Address, Phone) are required.");
            }

            var result = await _schoolService.CreateSchoolAndAddCoachAsync(userId, request.Name, request.Address, request.Phone);

            if (result.Contains("already a member"))
            {
                return Conflict(result); // Send conflict if user is already a member of another school
            }

            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize]
        public async Task<IActionResult> UpdateSchool([FromBody] SchoolUpdateRequest request)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            var success = await _schoolService.UpdateSchoolByCoachIdAsync(userId, request.Name, request.Address, request.Phone);
            if (!success)
            {
                return BadRequest("Failed to update the school. Ensure you have the right permissions.");
            }

            return Ok("School updated successfully.");
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSchool(int id)
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            var success = await _schoolService.DeleteSchoolAsync(userId, id);
            if (!success)
            {
                return BadRequest("Failed to delete the school. Ensure no users are associated and you have the right permissions.");
            }

            return Ok("School deleted successfully.");
        }

        [HttpGet("students")]
        [Authorize]
        public async Task<IActionResult> GetStudentsByCoach()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User not logged in.");
            }

            var students = await _schoolService.GetStudentsByCoachIdAsync(userId);
            if (students == null || students.Count == 0)
            {
                return NotFound("No students found for this coach.");
            }

            return Ok(students);
        }
    }
}
