namespace BjjTrainer.Models.Lessons
{
    public class LessonSection
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Difficulty { get; set; }
        public int Order { get; set; }
        public int LessonId { get; set; }
    }
}
