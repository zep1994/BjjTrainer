using BjjTrainer_API.Models.DTO.TrainingLogDTOs;
using BjjTrainer_API.Models.Trainings;
using BjjTrainer_API.Services_API.Trainings;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers.Training
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainingLogController : ControllerBase
    {
        private readonly TrainingService _trainingService;

        public TrainingLogController(TrainingService trainingService)
        {
            _trainingService = trainingService;
        }

        // ******************************** CREATE TRAINING LOG ********************************
        [HttpPost("log")]
        public async Task<IActionResult> CreateTrainingLog([FromBody] CreateTrainingLogDto dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.ApplicationUserId))
                return BadRequest("Invalid training log data.");

            await _trainingService.AddTrainingLogAsync(dto);
            return Ok();
        }

        // ******************************** GET TRAINING LOGS BY USER ********************************
        [HttpGet("list/{userId}")]
        public async Task<IActionResult> GetTrainingLogs(string userId)
        {
            try
            {
                var logs = await _trainingService.GetTrainingLogsAsync(userId);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ******************************** GET TRAINING LOG BY ID ********************************
        [HttpGet("{logId}")]
        public async Task<IActionResult> GetTrainingLogById(int logId)
        {
            try
            {
                var log = await _trainingService.GetTrainingLogByIdAsync(logId);
                if (log == null)
                    return NotFound(new { message = "Training log not found." });

                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ******************************** GET MOVES FOR A TRAINING LOG ********************************
        [HttpGet("log/{logId}")]
        public async Task<IActionResult> GetTrainingLogMovesAsync(int logId)
        {
            try
            {
                var log = await _trainingService.GetTrainingLogMovesAsync(logId);
                if (log == null)
                    return NotFound(new { message = "Training log not found." });

                return Ok(log);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ******************************** FETCH MOVES BY ID LIST ********************************
        [HttpPost("moves/byIds")]
        public async Task<IActionResult> GetMovesByIds([FromBody] List<int> moveIds)
        {
            if (moveIds == null || !moveIds.Any())
                return BadRequest("Move IDs list cannot be empty.");

            try
            {
                var moves = await _trainingService.GetMovesByIdsAsync(moveIds);
                if (!moves.Any())
                    return NotFound(new { message = "No moves found for the provided IDs." });

                return Ok(moves);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ******************************** UPDATE TRAINING LOG ********************************
        [HttpPut("{logId}")]
        public async Task<IActionResult> UpdateTrainingLog(int logId, [FromBody] UpdateTrainingLogDto dto)
        {
            if (dto == null || !dto.Moves.Any())
            {
                return BadRequest("Training log must include valid moves.");
            }

            try
            {
                await _trainingService.UpdateTrainingLogAsync(logId, dto);
                return Ok(new { message = "Training log updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ******************************** DELETE TRAINING LOG ********************************
        [HttpDelete("{logId}")]
        public async Task<IActionResult> DeleteTrainingLog(int logId)
        {
            try
            {
                await _trainingService.DeleteTrainingLogAsync(logId);
                return Ok(new { message = "Training log deleted successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ******************************** DELETE TRAINING LOG MOVE ********************************
        [HttpDelete("{logId}/removemove/{moveId}")]
        public async Task<IActionResult> RemoveMoveFromTrainingLog(int logId, int moveId)
        {
            try
            {
                await _trainingService.RemoveTrainingLogMoveAsync(logId, moveId);
                return Ok(new { message = "Move removed successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
