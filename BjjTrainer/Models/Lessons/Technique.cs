namespace BjjTrainer.Models.Lessons
{
    public class Technique
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public string? Type { get; set; } = string.Empty;

        public string? Content { get; set; } = string.Empty;

        public string? Belt { get; set; } = string.Empty;

        public string? SkillLevel { get; set; } = string.Empty; // Example: Beginner, Intermediate, Advanced

        public string? Category { get; set; } = string.Empty; // Example: Submission, Escape, Sweep

        public string? Position { get; set; } = string.Empty; // Example: Guard, Side Control, Mount

        public bool IsGi { get; set; }

        public List<string> Tags { get; set; } = new List<string>();

        public List<string>? CounterMoves { get; set; }

        public string? SubmissionType { get; set; } // Example: "Arm Lock", "Choke"

        public List<string>? Drills { get; set; }

        public string? SafetyTips { get; set; }

        public List<string>? References { get; set; }

        public int? EffectivenessRating { get; set; }

        public string? EnergyExpenditure { get; set; } // Example: "Low", "Medium", "High"

        public string? Origin { get; set; }

        public bool? LegalInCompetitions { get; set; }
    }
}
