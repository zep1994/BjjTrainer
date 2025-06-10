using BjjTrainer_API.Models.Users;

namespace BjjTrainer_API.Models.Calendars
{
    public class CalendarEventUser
    {
        public int CalendarEventId { get; set; }
        public CalendarEvent? CalendarEvent { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        public bool IsCheckedIn { get; set; } = false;
    }
}
