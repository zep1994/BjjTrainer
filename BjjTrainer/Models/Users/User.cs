namespace BjjTrainer.Models.Users
{
    public class User
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Belt { get; set; }
        public int BeltStripes { get; set; }
        public int TotalSubmissions { get; set; }
        public int TotalTaps { get; set; }
        public double TotalTrainingTime { get; set; }
        public int TotalRoundsRolled { get; set; }
        public int TrainingHoursThisWeek { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? PreferredTrainingStyle { get; set; }
    }
}
