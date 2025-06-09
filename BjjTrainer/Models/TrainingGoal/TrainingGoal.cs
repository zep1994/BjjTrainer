using BjjTrainer.Models.Users;

namespace BjjTrainer.Models.TrainingGoal
{
    public class TrainingGoal
    {
        public int Id { get; set; }
        public DateTime GoalDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public ICollection<UserTrainingGoalMove>? UserTrainingGoalMoves { get; set; }
    }
}
