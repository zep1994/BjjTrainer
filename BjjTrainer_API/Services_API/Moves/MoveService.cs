using BjjTrainer_API.Data;
using BjjTrainer_API.Models.DTO;
using BjjTrainer_API.Models.Moves;
using Microsoft.EntityFrameworkCore;

namespace BjjTrainer_API.Services_API.Moves
{
    public class MoveService(ApplicationDbContext context)
    {
        private readonly ApplicationDbContext _context = context;

        // ******************************** GET ALL MOVES ********************************
        public async Task<List<MoveDto>> GetAllMovesAsync()
        {
            return await _context.Moves
                .Select(m => new MoveDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description ?? string.Empty, 
                    SkillLevel = m.SkillLevel ?? string.Empty,  
                    Tags = m.Tags ?? new List<string>()         
                }).ToListAsync();
        }

        // ******************************** GET MOVE BY ID ********************************
        public async Task<MoveDto?> GetMoveByIdAsync(int id) 
        {
            var move = await _context.Moves
                .Include(m => m.SubLessonMoves)
                .ThenInclude(slm => slm.SubLesson)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (move == null) return null;

            return new MoveDto
            {
                Id = move.Id,
                Name = move.Name,
                Description = move.Description ?? string.Empty,
                Content = move.Content ?? string.Empty,
                SkillLevel = move.SkillLevel ?? string.Empty,
                Tags = move.Tags ?? []
            };
        }

        // ******************************** GET MOVES BY IDS ********************************
        public async Task<List<MoveDto>> GetMovesByIdsAsync(List<int> moveIds)
        {
            if (moveIds == null || !moveIds.Any())
                throw new ArgumentException("Move IDs list cannot be empty.");

            return await _context.Moves
                .Where(m => moveIds.Contains(m.Id))
                .Select(m => new MoveDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    Description = m.Description ?? string.Empty, 
                    SkillLevel = m.SkillLevel ?? string.Empty    
                }).ToListAsync();
        }

        // ******************************** CREATE MOVE ********************************
        public async Task<Move> CreateMoveAsync(Move move)
        {
            if (move == null)
                throw new ArgumentNullException(nameof(move), "Move cannot be null.");

            _context.Moves.Add(move);
            await _context.SaveChangesAsync();
            return move;
        }

        // ******************************** UPDATE MOVE ********************************
        public async Task<bool> UpdateMoveAsync(MoveDto moveDto)
        {
            var move = await _context.Moves.FindAsync(moveDto.Id);
            if (move == null) return false;

            move.Name = moveDto.Name;
            move.Description = moveDto.Description;
            move.Content = moveDto.Content;
            move.SkillLevel = moveDto.SkillLevel;
            move.Tags = moveDto.Tags;

            _context.Entry(move).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        // ******************************** DELETE MOVE ********************************
        public async Task<bool> DeleteMoveAsync(int id)
        {
            var move = await _context.Moves.FindAsync(id);
            if (move == null) return false;

            _context.Moves.Remove(move);
            await _context.SaveChangesAsync();
            return true;
        }

        // ******************************** GET MOVES BY SUBLESSON ID ********************************
        public async Task<List<MoveDto>> GetMovesBySubLessonIdAsync(int subLessonId)
        {
            return await _context.SubLessonMoves
                .Where(slm => slm.SubLessonId == subLessonId)
                .Select(slm => new MoveDto
                {
                    Id = slm.Move.Id,
                    Name = slm.Move.Name,
                    Description = slm.Move.Description ?? string.Empty, 
                    Content = slm.Move.Content ?? string.Empty,         
                    SkillLevel = slm.Move.SkillLevel ?? string.Empty,   
                    Tags = slm.Move.Tags ?? new List<string>()          
                })
                .ToListAsync();
        }
    }
}
