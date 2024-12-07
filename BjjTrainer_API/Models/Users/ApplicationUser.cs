﻿using BjjTrainer_API.Models.Calendars;
using BjjTrainer_API.Models.Goals;
using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Models.Moves;
using BjjTrainer_API.Models.Trainings;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.Users
{
    [Table("ApplicationUsers")]
    public class ApplicationUser : IdentityUser
    {
        // Navigation property for the one-to-many relationship
        public ICollection<CalendarEvent> CalendarEvents { get; set; } = new HashSet<CalendarEvent>();
        // Track lessons the user has engaged with
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();

        // Track the moves the user has learned or practiced
        public ICollection<Move> Moves { get; set; } = new List<Move>();

        public ICollection<TrainingLog> TrainingLogs { get; set; } = new List<TrainingLog>();
        public ICollection<TrainingGoal> TrainingGoals { get; set; } = new List<TrainingGoal>();


        [Column(TypeName = "date")]
        public DateOnly? TrainingStartDate { get; set; }
        public int TotalSubmissions { get; set; } = 0;
        public int TotalTaps { get; set; } = 0;
        public double TotalTrainingTime { get; set; } = 0; // HH.MM
        public int TotalRoundsRolled { get; set; } = 0;
        public string Belt { get; set; } = "White";
        public int BeltStripes { get; set; } = 0;
        public string ProfilePictureUrl { get; set; } = string.Empty;  
        public int TrainingHoursThisWeek { get; set; } = 0; 
        [Column(TypeName = "date")]
        public DateOnly? LastLoginDate { get; set; } 
        public string PreferredTrainingStyle { get; set; } = "Half-Gaurd"; 
    }
}