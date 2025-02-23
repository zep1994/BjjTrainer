using BjjTrainer.Models.DTO.Moves;

namespace BjjTrainer.Models.DTO.TrainingGoals
{
    public class TrainingGoalDto
    {
        public int Id { get; set; } // Unique identifier for the training goal
        public string ApplicationUserId { get; set; } = string.Empty; // Associated user ID
        public DateTime GoalDate { get; set; } = DateTime.Now; // Date for the training goal
        public string Notes { get; set; } = string.Empty; // Notes about the training goal
        public List<int> MoveIds { get; set; } = []; // IDs of the associated moves
        public List<MoveDto> Moves { get; set; } = []; // Details of associated moves
    }
}
