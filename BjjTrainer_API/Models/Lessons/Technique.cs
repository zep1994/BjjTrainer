using System.ComponentModel.DataAnnotations;

namespace BjjTrainer_API.Models.Lessons
{
    public class Technique
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Type cannot exceed 50 characters.")]
        public string? Type { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "Content cannot exceed 2000 characters.")]
        public string? Content { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Belt cannot exceed 20 characters.")]
        public string? Belt { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Skill Level cannot exceed 20 characters.")]
        public string? SkillLevel { get; set; } = string.Empty; // Example: Beginner, Intermediate, Advanced

        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        public string? Category { get; set; } = string.Empty; // Example: Submission, Escape, Sweep

        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters.")]
        public string? Position { get; set; } = string.Empty; // Example: Guard, Side Control, Mount

        public bool? IsGi { get; set; } = false;

        public List<string>? Tags { get; set; } = new List<string>(); // Keywords for filtering

        public List<string>? CounterMoves { get; set; }

        [StringLength(50, ErrorMessage = "Submission Type cannot exceed 50 characters.")]
        public string? SubmissionType { get; set; } // Example: "Arm Lock", "Choke"

        public List<string>? Drills { get; set; }

        [StringLength(500, ErrorMessage = "Safety Tips cannot exceed 500 characters.")]
        public string? SafetyTips { get; set; }

        public List<string>? References { get; set; }

        [Range(1, 10, ErrorMessage = "Effectiveness Rating must be between 1 and 10.")]
        public int? EffectivenessRating { get; set; }

        [StringLength(20, ErrorMessage = "Energy Expenditure cannot exceed 20 characters.")]
        public string? EnergyExpenditure { get; set; } // Example: "Low", "Medium", "High"

        [StringLength(100, ErrorMessage = "Origin cannot exceed 100 characters.")]
        public string? Origin { get; set; }

        public bool? LegalInCompetitions { get; set; }
    }
}
