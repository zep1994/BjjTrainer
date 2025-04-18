﻿using BjjTrainer_API.Models.Joins;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.Moves
{
    public class Move
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string? Description { get; set; } = string.Empty;

        [StringLength(5000, ErrorMessage = "Content cannot exceed 5000 characters.")]
        public string? Content { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Skill Level cannot exceed 20 characters.")]
        public string? SkillLevel { get; set; } = string.Empty; // Example: Beginner, Intermediate, Advanced

        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters.")]
        public string? Category { get; set; } = string.Empty; // Example: Submission, Escape, Sweep

        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters.")]
        public string? StartingPosition { get; set; } = string.Empty; // Example: Guard, Side Control, Mount

        [StringLength(2000, ErrorMessage = "Historical context cannot exceed 2000 characters.")]
        public string? History { get; set; } = string.Empty; // Background or origin of the move

        [StringLength(1000, ErrorMessage = "Counter strategies cannot exceed 1000 characters.")]
        public string? Scenarios { get; set; } = string.Empty; // Scenarios of when this move would be used

        [StringLength(1000, ErrorMessage = "Counter strategies cannot exceed 1000 characters.")]
        public string? CounterStrategies { get; set; } = string.Empty; // How to counter or defend against this move
        public List<string>? Tags { get; set; } = []; // Keywords for filtering
        public bool? LegalInCompetitions { get; set; } // Legal in Comp?                                             
        public int TrainingLogCount { get; set; } = 0;  // Track how many training logs include this move

        public ICollection<SubLessonMove> SubLessonMoves { get; set; } = [];
        // Relationship with TrainingLogs
        public ICollection<TrainingLogMove> TrainingLogMoves { get; set; } = [];
        public ICollection<UserTrainingGoalMove> UserTrainingGoalMoves { get; set; } = [];

    }
}
