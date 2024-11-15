using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Services_API;
using Microsoft.AspNetCore.Mvc;

namespace BjjTrainer_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechniqueController : ControllerBase
    {
        private readonly TechniqueService _techniqueService;

        public TechniqueController(TechniqueService techniqueService)
        {
            _techniqueService = techniqueService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Technique>>> GetAllTechniques()
        {
            var techniques = await _techniqueService.GetAllTechniquesAsync();
            return Ok(techniques);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Technique>> GetTechniqueById(int id)
        {
            var technique = await _techniqueService.GetTechniqueByIdAsync(id);
            if (technique == null) return NotFound();
            return Ok(technique);
        }

        [HttpPost]
        public async Task<ActionResult<Technique>> CreateTechnique(Technique technique)
        {
            var createdTechnique = await _techniqueService.CreateTechniqueAsync(technique);
            return CreatedAtAction(nameof(GetTechniqueById), new { id = createdTechnique.Id }, createdTechnique);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTechnique(int id, Technique technique)
        {
            if (id != technique.Id) return BadRequest();
            await _techniqueService.UpdateTechniqueAsync(technique);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTechnique(int id)
        {
            await _techniqueService.DeleteTechniqueAsync(id);
            return NoContent();
        }
    }
}
