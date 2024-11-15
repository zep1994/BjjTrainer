using BjjTrainer.Models.Lessons;
using BjjTrainer.Services.Lessons;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.Lesson
{
    public class LessonSectionViewModel : BaseViewModel
    {
        private readonly int _lessonId;
        private readonly LessonService _lessonService;

        public ObservableCollection<LessonSection> LessonSections { get; set; } = [];
        public string LessonTitle { get; }

        public LessonSectionViewModel(int lessonId, string lessonTitle)
        {
            _lessonId = lessonId;
            LessonTitle = lessonTitle;
            _lessonService = new LessonService();
        }

        public async Task LoadLessonSectionsAsync()
        {
            var lessonSections = await _lessonService.GetLessonSectionsAsync(_lessonId);
            if (lessonSections != null)
            {
                LessonSections.Clear();
                foreach (var section in lessonSections)
                {
                    LessonSections.Add(section);
                }
            }
        }
    }
}
