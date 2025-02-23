using BjjTrainer.Models.DTO.Moves;

namespace BjjTrainer.Models.DTO.TrainingLog
{
    public class TrainingLogDto
    {
        public int Id { get; set; }
        public string? ApplicationUserId { get; set; }
        public DateTime Date { get; set; }
        public double TrainingTime { get; set; }
        public int RoundsRolled { get; set; }
        public int Submissions { get; set; }
        public int Taps { get; set; }
        public string? Notes { get; set; }
        public string? SelfAssessment { get; set; }
        public List<LogMoveDto>? Moves { get; set; }
        public bool IsCoachLog { get; set; } = false;
        public List<int> MoveIds { get; set; } = [];


        public string DayColor => GetDayColor(Date.DayOfWeek);
        public string DayInitial => Date.DayOfWeek.ToString().Substring(0, 1).ToUpper();

        private string GetDayColor(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => "#4A90E2",   // Cool Blue
                DayOfWeek.Tuesday => "#50E3C2",  // Teal
                DayOfWeek.Wednesday => "#7B61FF", // Soft Purple
                DayOfWeek.Thursday => "#6495ED", // Cornflower Blue
                DayOfWeek.Friday => "#6A5ACD",   // Slate Blue
                DayOfWeek.Saturday => "#4682B4", // Steel Blue
                DayOfWeek.Sunday => "#708090",   // Slate Gray
                _ => "#FFFFFF"
            };
        }
    }
}