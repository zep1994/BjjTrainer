using BjjTrainer_API.Models.DTO.Moves;
using System.Collections.ObjectModel;

namespace BjjTrainer_API.Models.DTO.TrainingLogDTOs
{
    public class UpdateTrainingLogDto
    {
        public string? ApplicationUserId { get; set; }
        public DateTime? Date { get; set; }
        public double? TrainingTime { get; set; }
        public int? RoundsRolled { get; set; }
        public int? Submissions { get; set; }
        public int? Taps { get; set; }
        public string? Notes { get; set; }
        public string? SelfAssessment { get; set; }
        public bool? IsCoachLog { get; set; }
        public ObservableCollection<UpdateMoveDto>? Moves { get; set; }
    }
}
