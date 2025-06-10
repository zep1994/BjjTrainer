using BjjTrainer_API.Models.Calendars;
using BjjTrainer_API.Models.DTO.TrainingLogDTOs;

namespace BjjTrainer_API.Models.DTO.Calendars
{
    public class PastEventDetailsDto
    {
        public CalendarEvent? Event { get; set; }
        public List<CheckInDetailsDto>? CheckIns { get; set; }
        public TrainingLogDto? TrainingLog { get; set; }
    }
}
