using BjjTrainer_API.Models.Schools;
using BjjTrainer_API.Models.Trainings;
using BjjTrainer_API.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BjjTrainer_API.Models.Calendars
{
    public class CalendarEvent
    {
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = "Event";

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [Column(TypeName = "date")]
        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [Column(TypeName = "time")]
        [JsonPropertyName("startTime")]
        public TimeSpan? StartTime { get; set; }

        [Column(TypeName = "date")]
        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "time")]
        [JsonPropertyName("endTime")]
        public TimeSpan? EndTime { get; set; }

        [JsonPropertyName("isAllDay")]
        public bool IsAllDay { get; set; } = false;

        [JsonPropertyName("schoolId")]
        public int? SchoolId { get; set; }
        public School? School { get; set; }
        public bool IncludeTrainingLog { get; set; } = false;
        public int? TrainingLogId { get; set; } 
        [ForeignKey("TrainingLogId")]
        public TrainingLog? TrainingLog { get; set; }

        public ICollection<CalendarEventUser> CalendarEventUsers { get; set; } = [];
        public ICollection<CalendarEventCheckIn> CalendarEventCheckIns { get; set; } = [];

        public string? InstructorId { get; set; }
        public ApplicationUser? Instructor { get; set; }
    }
}
