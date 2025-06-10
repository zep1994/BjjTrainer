using BjjTrainer.Models.DTO.TrainingLog;

namespace BjjTrainer.Models.DTO.Events
{
    public class PastEventDetails
    {
        public CalendarEventDto? Event { get; set; }
        public List<CheckInDetails>? CheckIns { get; set; }
        public TrainingLogDto? TrainingLog { get; set; }
    }

}
