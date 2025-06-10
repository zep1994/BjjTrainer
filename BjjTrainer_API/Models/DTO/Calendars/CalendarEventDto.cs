namespace BjjTrainer_API.Models.DTO.Calendars
{
    public class CalendarEventDto
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool? IsAllDay { get; set; }
        public int? SchoolId { get; set; }
        public int? TrainingLogId { get; set; }
        public string? InstructorId { get; set; }
    }
}
