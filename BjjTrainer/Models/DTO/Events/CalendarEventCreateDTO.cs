namespace BjjTrainer.Models.DTO.Events
{
    public class CalendarEventCreateDTO
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan EndTime { get; set; }
        public string ApplicationUserId { get; set; }
        public bool IsAllDay { get; set; }
        public int? SchoolId { get; set; } = null;
        public bool IncludeTrainingLog { get; set; } = false;

        public List<int> MoveIds { get; set; } = [];


    }
}
