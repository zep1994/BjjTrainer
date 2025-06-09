namespace BjjTrainer_API.Models.DTO.UserDtos
{
    public class UserProgressDto
    {
        public double TotalTrainingTime { get; set; }
        public int TotalRoundsRolled { get; set; }
        public int TotalSubmissions { get; set; }
        public int TotalTaps { get; set; }
        public double WeeklyTrainingHours { get; set; } = new double();
        public double AverageSessionLength { get; set; }
        public string FavoriteMoveThisMonth { get; set; } = "No Moves Trained This Month";
        public int TotalGoalsAchieved { get; set; } = 0;
        public int TotalMoves { get; set; } = 0;
        public List<MoveDto>? MovesPerformed { get; set; }
    }
}
