using BjjTrainer_API.Models.Moves;
using BjjTrainer_API.Models.Trainings;
using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.Joins
{
    public class TrainingLogMove
    {
        public int TrainingLogId { get; set; }
        [ForeignKey("TrainingLogId")]
        public TrainingLog? TrainingLog { get; set; }

        public int MoveId { get; set; }
        public Move? Move { get; set; }

        // Distinguish coach-selected moves from student additions
        public bool IsCoachSelected { get; set; } = false;
    }
}
