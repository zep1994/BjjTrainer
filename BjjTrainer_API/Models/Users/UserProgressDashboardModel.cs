using BjjTrainer_API.Models.Moves;

namespace BjjTrainer_API.Models.Users
{
    public class UserProgressDashboardModel
    {
        public int TotalRoundsRolled { get; set; }
        public int TotalSubmissions { get; set; }
        public int TotalTaps { get; set; }
        public int TotalMoves { get; set; }
        public double TotalTrainingTime { get; set; }
        public List<MoveProgressModel>? Moves { get; set; }
    }
}
