using System.ComponentModel.DataAnnotations;

namespace BjjTrainer_API.Models.DTO.Calendars
{
    public class CreateEventDto
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public TimeSpan? StartTime { get; set; }

        public DateTime? EndDate { get; set; }
        public TimeSpan? EndTime { get; set; }

        public bool IsAllDay { get; set; } = false;

        // Is this a training session?
        public bool IncludeTrainingLog { get; set; } = false;

        // Moves selected by the coach (optional)
        public List<int> MoveIds { get; set; } = [];

        public string? InstructorId { get; set; } // <-- Add this
    }
}
