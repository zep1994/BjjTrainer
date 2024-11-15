namespace BjjTrainer.Models.Lessons
{
    public class SubLesson
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(1);
        public string VideoUrl { get; set; } = string.Empty;
        public string DocumentUrl { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public string SkillLevel { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int? LessonSectionId { get; set; }
        public LessonSection? LessonSection { get; set; }
    }

}
