using BjjTrainer_API.Models.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.Calendars
{
    public class CalendarEventCheckIn
    {
        public int Id { get; set; }
        public int CalendarEventId { get; set; }
        public CalendarEvent? CalendarEvent { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }

        [Column(TypeName = "date")]
        public DateTime CheckInTime { get; set; }
    }

}
