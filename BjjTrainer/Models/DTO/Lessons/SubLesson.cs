using BjjTrainer.Models.Moves;
using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer.Models.Lessons
{
    public class SubLesson
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public TimeSpan Duration { get; set; }
        public string VideoUrl { get; set; } = string.Empty; 
        public string DocumentUrl { get; set; } = string.Empty; 
        public List<string> Tags { get; set; } = []; 
        public string SkillLevel { get; set; } = string.Empty; 
        public string Notes { get; set; } = string.Empty; 

        [ForeignKey("LessonSection")]
        public int LessonSectionId { get; set; }
        public LessonSection? LessonSection { get; set; }

        public List<MoveDto> Moves { get; set; } = []; 
    }

}
