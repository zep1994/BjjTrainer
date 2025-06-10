using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Services_API.Lessons;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers.Lessons
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonSectionsController : ControllerBase
    {
        private readonly LessonSectionService _lessonSectionService;

        public LessonSectionsController(LessonSectionService lessonSectionService)
        {
            _lessonSectionService = lessonSectionService;
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<IEnumerable<LessonSection>>> GetSectionsByLesson(int lessonId)
        {
            var sections = await _lessonSectionService.GetSectionsByLessonIdAsync(lessonId);
            return Ok(sections);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonSection>> GetSection(int id)
        {
            var section = await _lessonSectionService.GetSectionByIdAsync(id);
            return section == null ? (ActionResult<LessonSection>)NotFound() : (ActionResult<LessonSection>)Ok(section);
        }

        [HttpPost]
        public async Task<ActionResult<LessonSection>> CreateSection(LessonSection section)
        {
            var createdSection = await _lessonSectionService.CreateSectionAsync(section);
            return CreatedAtAction(nameof(GetSection), new { id = createdSection.Id }, createdSection);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSection(int id, LessonSection section)
        {
            if (id != section.Id) return BadRequest();
            await _lessonSectionService.UpdateSectionAsync(section);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSection(int id)
        {
            var deleted = await _lessonSectionService.DeleteSectionAsync(id);
            return !deleted ? NotFound() : NoContent();
        }
    }
}
