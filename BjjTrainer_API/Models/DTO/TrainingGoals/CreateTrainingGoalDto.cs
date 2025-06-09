using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.DTO.TrainingGoals
{
    public class CreateTrainingGoalMoveDto
    {
        public int MoveId { get; set; }
        public int PracticeCount { get; set; }
    }

    public class CreateTrainingGoalDto
    {
        public required string ApplicationUserId { get; set; }
        [Column(TypeName = "date")]
        public DateTime GoalDate { get; set; }
        public string? Notes { get; set; } = string.Empty;
        public List<CreateTrainingGoalMoveDto> Moves { get; set; } = [];
    }
}
