using System.Collections.ObjectModel;
using BjjTrainer.Models.DTO.Moves;

namespace BjjTrainer.Models.DTO.TrainingLog
{
    public class UpdateTrainingLogDto
    {
        public int? Id { get; set; }
        public DateTime Date { get; set; }
        public double TrainingTime { get; set; }
        public int RoundsRolled { get; set; }
        public int Submissions { get; set; }
        public int Taps { get; set; }
        public string? Notes { get; set; }
        public string? SelfAssessment { get; set; }
        public bool IsCoachLog { get; set; }
        public ObservableCollection<UpdateMoveDto> Moves { get; set; } = [];
        public string? ApplicationUserId { get; set; }
    }
}
