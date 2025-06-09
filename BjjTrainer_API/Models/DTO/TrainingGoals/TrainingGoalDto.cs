using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.DTO.TrainingGoals
{
    public class TrainingGoalMoveDto
    {
        public int MoveId { get; set; }
        public int PracticeCount { get; set; }
        public MoveDto? Move { get; set; }
    }

    public class TrainingGoalDto
    {
        public int Id { get; set; }
        public DateTime GoalDate { get; set; }
        public string? Notes { get; set; }
        public List<TrainingGoalMoveDto>? Moves { get; set; }
        public List<int>? MoveIds { get; set; }
    }
}
