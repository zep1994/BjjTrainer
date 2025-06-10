using System.ComponentModel.DataAnnotations;

namespace BjjTrainer.Models.Lessons
{
    public class Lesson
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = "Lesson";
        [Required]
        public string Belt { get; set; } = "White";
        [Required]
        public string Category { get; set; } = "Basics";
        [Required]
        public string Difficulty { get; set; } = "Starter";
        [Required]
        public string Description { get; set; } = "Description";
    }
}
