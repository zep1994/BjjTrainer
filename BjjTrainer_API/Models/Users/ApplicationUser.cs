using BjjTrainer_API.Models.Calendars;
using BjjTrainer_API.Models.Goals;
using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Models.Moves;
using BjjTrainer_API.Models.Schools;
using BjjTrainer_API.Models.Trainings;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.Users
{
    public enum UserRole
    {
        Student,
        Coach,
        Instructor,
        Owner,
        Admin
    }

    [Table("ApplicationUsers")]
    public class ApplicationUser : IdentityUser
    {
        public ICollection<CalendarEventUser> CalendarEventUsers { get; set; } = [];
        public ICollection<Lesson> Lessons { get; set; } = [];
        public ICollection<Move> Moves { get; set; } = [];
        public ICollection<TrainingLog> TrainingLogs { get; set; } = [];
        public ICollection<TrainingGoal> TrainingGoals { get; set; } = [];


        [Column(TypeName = "date")]
        public DateOnly? TrainingStartDate { get; set; }
        public int TotalSubmissions { get; set; } = 0;
        public int TotalTaps { get; set; } = 0;
        public double TotalTrainingTime { get; set; } = 0; 
        public int TotalRoundsRolled { get; set; } = 0;
        public string Belt { get; set; } = "White";
        public string Stripes { get; set; } = "0"; 
        public int BeltStripes { get; set; } = 0;
        public string ProfilePictureUrl { get; set; } = string.Empty;  
        public int TrainingHoursThisWeek { get; set; } = 0; 


        [Column(TypeName = "date")]
        public DateOnly? LastLoginDate { get; set; } 
        public string PreferredTrainingStyle { get; set; } = "Half-Gaurd";


        public UserRole Role { get; set; } = UserRole.Student;
        public int? SchoolId { get; set; } 
        public School? School { get; set; }
        public string? StripeCustomerId { get; set; }
        public string? StripeSubscriptionId { get; set; }
        public string? MembershipPlan { get; set; } 
        public string? MembershipStatus { get; set; }
        public DateTime? MembershipStartDate { get; set; }
        public DateTime? MembershipEndDate { get; set; }
    }
}