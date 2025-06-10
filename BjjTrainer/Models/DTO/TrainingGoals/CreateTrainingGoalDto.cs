namespace BjjTrainer.Models.DTO.TrainingGoals
{
    public class CreateTrainingGoalMoveDto
    {
        public int? MoveId { get; set; }
        public int PracticeCount { get; set; }
    }

    public class CreateTrainingGoalDto
    {
        public string? ApplicationUserId { get; set; }
        public DateTime GoalDate { get; set; }
        public string? Notes { get; set; }
        public List<int>? MoveIds { get; set; } 
    }
}
