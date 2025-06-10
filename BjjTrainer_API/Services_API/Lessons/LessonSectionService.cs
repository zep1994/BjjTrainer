using BjjTrainer_API.Data;
using BjjTrainer_API.Models.Lessons;
using Microsoft.EntityFrameworkCore;

namespace BjjTrainer_API.Services_API.Lessons
{
    public class LessonSectionService
    {
        private readonly ApplicationDbContext _context;

        public LessonSectionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LessonSection>> GetSectionsByLessonIdAsync(int lessonId)
        {
            return await _context.LessonSections
                                 .Where(ls => ls.LessonId == lessonId)
                                 .ToListAsync();
        }

        public async Task<LessonSection?> GetSectionByIdAsync(int id)
        {
            return await _context.LessonSections.FindAsync(id);
        }

        public async Task<LessonSection> CreateSectionAsync(LessonSection section)
        {
            _context.LessonSections.Add(section);
            await _context.SaveChangesAsync();
            return section;
        }


        public async Task<LessonSection> UpdateSectionAsync(LessonSection section)
        {
            _context.Entry(section).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return section;
        }

        public async Task<bool> DeleteSectionAsync(int id)
        {
            var section = await _context.LessonSections.FindAsync(id);
            if (section == null) return false;

            _context.LessonSections.Remove(section);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
