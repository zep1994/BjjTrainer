namespace BjjTrainer.Models.DTO.Users
{
    public class DailyUserProgressDto
    {
        public DateTime Date { get; set; }
        public double Value { get; set; }
        public string Day => Date.ToString("ddd");
    }
}