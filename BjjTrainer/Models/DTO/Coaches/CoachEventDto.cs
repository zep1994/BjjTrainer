using BjjTrainer.Models.DTO.Events;
using BjjTrainer.Models.Moves;

namespace BjjTrainer.Models.DTO.Coaches
{
    public class CoachEventDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public int? SchoolId { get; set; }

        public List<CheckInDto> CheckIns { get; set; } = new();
        public List<LogMoveDto>? Moves { get; set; } = new();
    }
}
