using BjjTrainer.Models.Moves;

namespace BjjTrainer.Models.DTO.TrainingGoals
{
    public class TrainingGoalMoveDto
    {
        public int? MoveId { get; set; }
        public int PracticeCount { get; set; }
        public MoveDto? Move { get; set; }
    }

    public class TrainingGoalDto
    {
        public int Id { get; set; } 
        public DateTime GoalDate { get; set; } = DateTime.Now; 
        public string Notes { get; set; } = string.Empty; 
        public List<TrainingGoalMoveDto> Moves { get; set; } = []; 
    }
}
