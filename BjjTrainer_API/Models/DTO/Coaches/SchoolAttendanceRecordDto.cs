namespace BjjTrainer_API.Models.DTO.Coaches
{
    public class SchoolAttendanceRecordDto
    {
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public DateTime? EventDate { get; set; }
        public DateTime CheckInTime { get; set; }
        public int TotalEventsAttended { get; set; } // Add this property
    }
}
