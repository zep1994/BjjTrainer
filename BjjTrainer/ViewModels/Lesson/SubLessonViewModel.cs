using BjjTrainer.Models.Lessons;
using BjjTrainer.Services.Lessons;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels
{
    public partial class SubLessonViewModel : BaseViewModel
    {
        private readonly SubLessonService _subLessonService;
        private ObservableCollection<SubLesson> _subLessons;

        public ObservableCollection<SubLesson> SubLessons
        {
            get => _subLessons;
            set => SetProperty(ref _subLessons, value); // Ensure this is an ObservableCollection
        }

        public string SectionTitle { get; set; }

        public SubLessonViewModel(int lessonSectionId)
        {
            _subLessonService = new SubLessonService();
            _ = LoadSubLessonsAsync(lessonSectionId);
        }

        public async Task LoadSubLessonsAsync(int lessonSectionId)
        {
            try
            {
                var subLessons = await _subLessonService.GetSubLessonsBySectionAsync(lessonSectionId);
                SubLessons = new ObservableCollection<SubLesson>(subLessons);
            }
            catch (Exception ex)
            {
                // You can add logging here
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load sublessons.", "OK");
            }
        }
    }
}
