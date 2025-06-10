using BjjTrainer.Models.Users;

namespace BjjTrainer.Models.DTO.Events
{
    public class UserCalendarEvent
    {
        public int CalendarEventId { get; set; }
        public CalendarEventDto? CalendarEvent { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }

        public bool IsCheckedIn { get; set; } = false;
    }
}
