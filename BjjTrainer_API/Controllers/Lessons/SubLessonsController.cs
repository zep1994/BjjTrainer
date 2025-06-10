using BjjTrainer_API.Models.DTO.Lessons;
using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Services_API.Lessons;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers.Lessons
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubLessonsController : ControllerBase
    {
        private readonly SubLessonService _subLessonService;

        public SubLessonsController(SubLessonService subLessonService)
        {
            _subLessonService = subLessonService;
        }

        [HttpGet("sections/{lessonSectionId}")]
        public async Task<ActionResult<List<SubLesson>>> GetSubLessonsBySection(int lessonSectionId)
        {
            var subLessons = await _subLessonService.GetSubLessonsBySectionAsync(lessonSectionId);

            if (subLessons == null || subLessons.Count == 0)
            {
                return NotFound("No sub-lessons found for the provided section.");
            }

            return Ok(subLessons); // Return the actual list of sub-lessons
        }

        // Get a specific SubLesson by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<SubLesson>> GetSubLesson(int id)
        {
            var subLesson = await _subLessonService.GetSubLessonByIdAsync(id);
            return subLesson == null ? (ActionResult<SubLesson>)NotFound() : (ActionResult<SubLesson>)Ok();
        }

        // Create a new SubLesson
        [HttpPost]
        public async Task<ActionResult<SubLesson>> CreateSubLesson([FromBody] SubLessonCreateDto subLessonDto)
        {
            if (subLessonDto == null)
            {
                return BadRequest("SubLesson data is required.");
            }

            // Map DTO to SubLesson model
            var subLesson = new SubLesson
            {
                Title = subLessonDto.Title,
                Content = subLessonDto.Content,
                Duration = subLessonDto.Duration,
                VideoUrl = subLessonDto.VideoUrl,
                DocumentUrl = subLessonDto.DocumentUrl,
                Tags = subLessonDto.Tags,
                SkillLevel = subLessonDto.SkillLevel,
                Notes = subLessonDto.Notes,
                LessonSectionId = subLessonDto.LessonSectionId
            };

            try
            {
                var createdSubLesson = await _subLessonService.CreateSubLessonAsync(subLesson, subLessonDto.MoveId);
                return CreatedAtAction(nameof(GetSubLesson), new { id = createdSubLesson.Id }, createdSubLesson);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete a SubLesson
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubLesson(int id)
        {
            var deleted = await _subLessonService.DeleteSubLessonAsync(id);
            return !deleted ? NotFound() : NoContent();
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<SubLessonDetailsDto>> GetSubLessonDetails(int id)
        {
            var subLessonDetails = await _subLessonService.GetSubLessonDetailsByIdAsync(id);

            if (subLessonDetails == null)
            {
                return NotFound("SubLesson details not found.");
            }

            return Ok(subLessonDetails); // Returning the DTO with the sub-lesson details
        }

        // Add a Move to a SubLesson
        [HttpPost("{subLessonId}/moves/{moveId}")]
        public async Task<IActionResult> AddMoveToSubLesson(int subLessonId, int moveId)
        {
            var success = await _subLessonService.AddMoveToSubLessonAsync(subLessonId, moveId);
            return !success ? BadRequest("Failed to associate Move with SubLesson.") : Ok(success);
        }

        // Remove a Move from a SubLesson
        [HttpDelete("{subLessonId}/moves/{moveId}")]
        public async Task<IActionResult> RemoveMoveFromSubLesson(int subLessonId, int moveId)
        {
            var success = await _subLessonService.RemoveMoveFromSubLessonAsync(subLessonId, moveId);
            return !success ? BadRequest("Failed to remove Move from SubLesson.") : Ok("This was Deleted");
        }
    }
}
