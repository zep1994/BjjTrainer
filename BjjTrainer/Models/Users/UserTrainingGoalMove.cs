using BjjTrainer.Models.DTO.Moves;

namespace BjjTrainer.Models.Users
{
    public class UserTrainingGoalMove
    {
        public int TrainingGoalId { get; set; }
        public int MoveId { get; set; }
        public MoveDto Move { get; set; }
    }
}
