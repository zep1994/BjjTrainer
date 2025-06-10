using BjjTrainer_API.Models.Lessons;
using BjjTrainer_API.Models.Moves;

namespace BjjTrainer_API.Models.Joins
{
    public class SubLessonMove
    {
        public int SubLessonId { get; set; }
        public SubLesson? SubLesson { get; set; }

        public int MoveId { get; set; }
        public Move? Move { get; set; }
    }
}
