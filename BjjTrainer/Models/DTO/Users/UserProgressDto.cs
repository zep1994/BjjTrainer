using BjjTrainer.Models.DTO.Moves;

namespace BjjTrainer.Models.DTO.Users
{
    public class UserProgressDto
    {
        public double TotalTrainingTime { get; set; }
        public int TotalRoundsRolled { get; set; }
        public int TotalSubmissions { get; set; }
        public int TotalTaps { get; set; }
        public double WeeklyTrainingHours { get; set; }
        public double AverageSessionLength { get; set; }
        public string FavoriteMoveThisMonth { get; set; }
        public int TotalGoalsAchieved { get; set; }
        public int TotalMoves { get; set; }
        public List<MoveDto> MovesPerformed { get; set; } = [];
    }
}
