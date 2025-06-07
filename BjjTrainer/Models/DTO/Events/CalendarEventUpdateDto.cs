using System.Text.Json.Serialization;

namespace BjjTrainer.Models.DTO.Events
{
    public class CalendarEventUpdateDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("startDate")]
        public DateTime? StartDate { get; set; }

        [JsonPropertyName("startTime")]
        public TimeSpan? StartTime { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime? EndDate { get; set; }

        [JsonPropertyName("endTime")]
        public TimeSpan? EndTime { get; set; }

        [JsonPropertyName("isAllDay")]
        public bool? IsAllDay { get; set; }

        [JsonPropertyName("schoolId")]
        public int? SchoolId { get; set; }
    }
}
