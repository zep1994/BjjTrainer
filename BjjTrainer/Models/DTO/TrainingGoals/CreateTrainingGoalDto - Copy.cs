namespace BjjTrainer.Models.DTO.TrainingGoals
{
    public class UpdateTrainingGoalMoveDto
    {
        public int MoveId { get; set; }
        public int PracticeCount { get; set; }
    }

    public class UpdateTrainingGoalDto
    {
        public string ApplicationUserId { get; set; }
        public DateTime GoalDate { get; set; }
        public string? Notes { get; set; }
        public List<UpdateTrainingGoalMoveDto>? Moves { get; set; } = [];
    }
}
