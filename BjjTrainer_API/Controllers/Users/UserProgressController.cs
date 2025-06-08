using BjjTrainer_API.Models.DTO.UserDtos;
using BjjTrainer_API.Services_API.Users;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers.Users
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProgressController : ControllerBase
    {
        private readonly UserProgressService _userProgressService;

        public UserProgressController(UserProgressService userProgressService)
        {
            _userProgressService = userProgressService;
        }

        [HttpGet("{userId}/progress")]
        public async Task<ActionResult<UserProgressDto>> GetUserProgress(string userId)
        {
            try
            {
                var progress = await _userProgressService.GetUserProgressAsync(userId);
                return Ok(progress);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An error occurred while fetching user progress.", error = ex.Message });
            }
        }
    }
}
