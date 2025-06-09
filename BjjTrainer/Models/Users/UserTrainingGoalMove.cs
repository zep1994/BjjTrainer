using BjjTrainer.Models.Moves;

namespace BjjTrainer.Models.Users
{
    public class UserTrainingGoalMove
    {
        public int TrainingGoalId { get; set; }
        public int MoveId { get; set; }
        public Move? Move { get; set; }
        public int PracticeCount { get; set; } = 1;
    }
}
