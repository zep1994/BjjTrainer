using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Services_API.Lessons;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers.Lessons
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonsController : ControllerBase
    {
        private readonly LessonService _lessonService;

        public LessonsController(LessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLessons()
        {
            var lessons = await _lessonService.GetAllLessonsAsync();
            return Ok(lessons);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLessonById(int id)
        {
            var lesson = await _lessonService.GetLessonByIdAsync(id);
            return lesson == null ? NotFound() : Ok(lesson);
        }

        [HttpPost]
        public async Task<IActionResult> CreateLesson([FromBody] Lesson lesson)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdLesson = await _lessonService.CreateLessonAsync(lesson);
            return CreatedAtAction(nameof(GetLessonById), new { id = createdLesson.Id }, createdLesson);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLesson(int id, Lesson lesson)
        {
            if (id != lesson.Id) return BadRequest();

            var result = await _lessonService.UpdateLessonAsync(lesson);
            return !result ? NotFound() : Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLesson(int id)
        {
            var result = await _lessonService.DeleteLessonAsync(id);
            return !result ? NotFound() : NoContent();
        }
    }
}