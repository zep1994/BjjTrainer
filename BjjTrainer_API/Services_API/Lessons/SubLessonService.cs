using BjjTrainer_API.Data;
using BjjTrainer_API.Models.DTO.Lessons;
using BjjTrainer_API.Models.Joins;
using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Models.Moves;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BjjTrainer_API.Services_API.Lessons
{
    public class SubLessonService
    {
        private readonly ApplicationDbContext _context;

        // Constructor-based injection for ApplicationDbContext
        public SubLessonService(ApplicationDbContext context)
        {
            _context = context;
        }
        // Get all SubLessons for a specific LessonSection
        public async Task<List<SubLesson>> GetSubLessonsBySectionAsync(int lessonSectionId)
        {
            try
            {
                var subLessons = await _context.SubLessons
                    .Where(sl => sl.LessonSectionId == lessonSectionId)
                    .ToListAsync();

                return subLessons;
            }
            catch (Exception ex)
            {
                // Handle and log error here (you may also return a custom error)
                throw new InvalidOperationException("Error fetching sub-lessons for the given section.", ex);
            }
        }

        public async Task<SubLessonDetailsDto> GetSubLessonDetailsByIdAsync(int id)
        {
            try
            {
                var subLesson = await _context.SubLessons
                    .Include(sl => sl.SubLessonMoves)
                    .ThenInclude(slm => slm.Move)
                    .FirstOrDefaultAsync(sl => sl.Id == id);

                if (subLesson == null) return null;

                return new SubLessonDetailsDto
                {
                    Id = subLesson.Id,
                    Title = subLesson.Title,
                    Moves = [.. subLesson.SubLessonMoves.Select(m => new Move
                    {
                        Id = m.Move.Id,
                        Name = m.Move.Name,
                        Description = m.Move.Description,
                        Content = m.Move.Content,
                        SkillLevel = m.Move.SkillLevel,
                        Category = m.Move.Category,
                        StartingPosition = m.Move.StartingPosition,
                        History = m.Move.History,
                        Scenarios = m.Move.Scenarios,
                        LegalInCompetitions = m.Move.LegalInCompetitions,
                        CounterStrategies = m.Move.CounterStrategies,
                        Tags = m.Move.Tags
                    })]
                };
            }
            catch (Exception ex)
            {
                // Handle and log error here
                throw new InvalidOperationException("Error fetching sub-lesson details.", ex);
            }
        }

        public async Task<bool> AddMoveToSubLessonAsync(int subLessonId, int moveId)
        {
            var subLesson = await _context.SubLessons.FindAsync(subLessonId);
            var move = await _context.Moves.FindAsync(moveId);

            if (subLesson == null || move == null) return false;

            if (!_context.SubLessonMoves.Any(slm => slm.SubLessonId == subLessonId && slm.MoveId == moveId))
            {
                _context.SubLessonMoves.Add(new SubLessonMove { SubLessonId = subLessonId, MoveId = moveId });
                await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveMoveFromSubLessonAsync(int subLessonId, int moveId)
        {
            var subLessonMove = await _context.SubLessonMoves
                .FirstOrDefaultAsync(slm => slm.SubLessonId == subLessonId && slm.MoveId == moveId);

            if (subLessonMove == null) return false;

            _context.SubLessonMoves.Remove(subLessonMove);
            await _context.SaveChangesAsync();

            return true;
        }

        // Get a specific SubLesson by ID
        public async Task<SubLesson> GetSubLessonByIdAsync(int id)
        {
            var subLesson = await _context.SubLessons
                                 .FirstOrDefaultAsync(sl => sl.Id == id);
            return subLesson;
        }

        // Create a new SubLesson
        public async Task<SubLesson> CreateSubLessonAsync(SubLesson subLesson, int? moveId)
        {
            // Save the SubLesson first
            _context.SubLessons.Add(subLesson);
            await _context.SaveChangesAsync();

            // If MoveId is provided, validate and add the relationship
            if (moveId.HasValue)
            {
                var moveExists = await _context.Moves.AnyAsync(m => m.Id == moveId.Value);
                if (!moveExists)
                {
                    throw new InvalidOperationException($"Move with Id {moveId.Value} does not exist.");
                }

                var subLessonMove = new SubLessonMove
                {
                    SubLessonId = subLesson.Id,
                    MoveId = moveId.Value
                };

                _context.SubLessonMoves.Add(subLessonMove);
                await _context.SaveChangesAsync();
            }

            return subLesson;
        }

        // Update an existing SubLesson
        public async Task<SubLesson> UpdateSubLessonAsync(SubLesson subLesson)
        {
            _context.Entry(subLesson).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return subLesson;
        }

        // Delete a SubLesson
        public async Task<bool> DeleteSubLessonAsync(int id)
        {
            var subLesson = await _context.SubLessons.FindAsync(id);
            if (subLesson == null) return false;

            _context.SubLessons.Remove(subLesson);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task<ActionResult<SubLessonDetailsDto>> GetSubLessonDetails(int id)
        {
            throw new NotImplementedException();
        }
    }
}