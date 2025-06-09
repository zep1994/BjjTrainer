using BjjTrainer_API.Models.DTO.TrainingGoals;
using BjjTrainer_API.Services_API.Goals;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers.Goals
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrainingGoalController : ControllerBase
    {
        private readonly TrainingGoalService _goalService;

        public TrainingGoalController(TrainingGoalService goalService)
        {
            _goalService = goalService;
        }

        [HttpGet("{goalId}")]
        public async Task<IActionResult> GetGoalById(int goalId)
        {
            try
            {
                var goal = await _goalService.GetTrainingGoalByIdAsync(goalId);
                return Ok(goal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGoal([FromBody] CreateTrainingGoalDto dto)
        {
            if (dto == null || dto.Moves == null || !dto.Moves.Any())
                return BadRequest("A training goal must include at least one move.");

            if (dto.Moves.Any(m => m.PracticeCount <= 0))
                return BadRequest("Each move must have a practice count greater than zero.");

            try
            {
                await _goalService.AddTrainingGoalAsync(dto);
                return Ok(new { message = "Training goal created successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetGoals(string userId)
        {
            try
            {
                var goals = await _goalService.GetGoalsByUserAsync(userId);
                return Ok(goals);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut("{goalId}")]
        public async Task<IActionResult> UpdateGoal(int goalId, [FromBody] CreateTrainingGoalDto dto)
        {
            if (dto == null || dto.Moves == null || !dto.Moves.Any())
                return BadRequest("A training goal must include at least one move.");

            if (dto.Moves.Any(m => m.PracticeCount <= 0))
                return BadRequest("Each move must have a practice count greater than zero.");

            try
            {
                await _goalService.UpdateTrainingGoalAsync(goalId, dto);
                return Ok(new { message = "Training goal updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("{goalId}")]
        public async Task<IActionResult> DeleteGoal(int goalId)
        {
            try
            {
                await _goalService.DeleteTrainingGoalAsync(goalId);
                return Ok(new { message = "Training goal deleted successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}