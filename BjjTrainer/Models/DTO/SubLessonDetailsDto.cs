using BjjTrainer.Models.Moves;

namespace BjjTrainer.Models.DTO
{
    public class SubLessonDetailsDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Notes { get; set; }

        // New property to hold the list of moves associated with this sublesson
        public List<MoveDto>? Moves { get; set; }
    }
}
