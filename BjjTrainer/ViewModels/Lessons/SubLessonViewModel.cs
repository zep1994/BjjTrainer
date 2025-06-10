using BjjTrainer.Models.Lessons;
using BjjTrainer.Services.Lessons;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels
{
    public partial class SubLessonViewModel : BaseViewModel
    {
        private readonly SubLessonService _subLessonService;
        private ObservableCollection<SubLesson> _subLessons = []; // Initialize to avoid null

        public ObservableCollection<SubLesson> SubLessons
        {
            get => _subLessons;
            set => SetProperty(ref _subLessons, value); // Ensure this is an ObservableCollection
        }

        public string SectionTitle { get; set; } = string.Empty; // Initialize to avoid null

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
                // Ensure Application.Current and Application.Current.MainPage are not null
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"{ex}", "OK");
                }
                else
                {
                    // Handle the case where MainPage is null (e.g., log the error)
                    System.Diagnostics.Debug.WriteLine($"Error: {ex}");
                }
            }
        }
    }
}
