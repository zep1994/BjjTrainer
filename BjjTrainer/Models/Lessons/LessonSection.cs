namespace BjjTrainer.Models.Lessons
{
    public class LessonSection
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public int Order { get; set; } = 0;
        public int LessonId { get; set; }
    }
}
