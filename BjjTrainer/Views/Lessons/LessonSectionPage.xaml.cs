using BjjTrainer.Models.Lessons;
using BjjTrainer.ViewModels.Lesson;

namespace BjjTrainer.Views.Lessons
{
    public partial class LessonSectionPage : ContentPage
    {
        private readonly LessonSectionViewModel _viewModel;

        public LessonSectionPage(int lessonId, string lessonTitle)
        {
            InitializeComponent();
            _viewModel = new LessonSectionViewModel(lessonId, lessonTitle);
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadLessonSectionsAsync();
        }

        public async void OnViewSubLessonClicked(object sender, EventArgs e)
        {
            // Check if sender is Button and get binding context as LessonSection
            if (sender is Button button && button.BindingContext is LessonSection lessonSection)
            {
                // Navigate to SubLessonDetailsPage, passing LessonSectionId and Title
                await Navigation.PushAsync(new SubLessonPage(lessonSection.Id));
            }
        }
    }
}
