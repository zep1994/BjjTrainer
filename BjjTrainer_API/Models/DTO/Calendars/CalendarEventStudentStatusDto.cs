namespace BjjTrainer_API.Models.DTO.Calendars
{
    public class CalendarEventStudentStatusDto
    {
        public string UserId { get; set; } = string.Empty;
        public string? UserName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public bool IsBooked { get; set; }
        public bool IsCheckedIn { get; set; }
    }
}
