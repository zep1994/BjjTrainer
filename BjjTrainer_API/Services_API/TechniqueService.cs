using BjjTrainer_API.Data;
using BjjTrainer_API.Models.Lessons;
using Microsoft.EntityFrameworkCore;

namespace BjjTrainer_API.Services_API
{
    public class TechniqueService
    {
        private readonly ApplicationDbContext _context;

        public TechniqueService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Technique>> GetAllTechniquesAsync()
        {
            return await _context.Techniques.ToListAsync();
        }

        public async Task<Technique?> GetTechniqueByIdAsync(int id)
        {
            return await _context.Techniques.FindAsync(id);
        }

        public async Task<Technique> CreateTechniqueAsync(Technique technique)
        {
            _context.Techniques.Add(technique);
            await _context.SaveChangesAsync();
            return technique;
        }

        public async Task UpdateTechniqueAsync(Technique technique)
        {
            _context.Entry(technique).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTechniqueAsync(int id)
        {
            var technique = await _context.Techniques.FindAsync(id);
            if (technique != null)
            {
                _context.Techniques.Remove(technique);
                await _context.SaveChangesAsync();
            }
        }
    }
}