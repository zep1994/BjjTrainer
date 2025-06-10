using BjjTrainer_API.Models.DTO.Moves;
using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.DTO.TrainingLogDTOs
{
    public class TrainingLogDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Column(TypeName = "time")]
        public TimeSpan? StartTime { get; set; }

        public string? ApplicationUserId { get; set; }
        public double TrainingTime { get; set; } = 0;
        public int RoundsRolled { get; set; } = 0;
        public int Submissions { get; set; } = 0;
        public int Taps { get; set; } = 0;
        public string Notes { get; set; } = string.Empty;
        public string SelfAssessment { get; set; } = string.Empty;
        public List<int>? MoveIds { get; set; } = [];
        public List<LogMoveDto>? Moves { get; set; } = [];
        public bool IsCoachLog { get; set; }
        public bool IsImported { get; set; } = false;
        public int? ImportedFromLogId { get; set; }
        public int? CalendarEventId { get; set; }
    }
}
