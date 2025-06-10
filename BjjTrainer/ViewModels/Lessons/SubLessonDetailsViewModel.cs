using BjjTrainer.Models.DTO;
using BjjTrainer.Models.Moves;
using BjjTrainer.Services.Lessons;
using MvvmHelpers;
using System.Diagnostics;
using System.Windows.Input;

namespace BjjTrainer.ViewModels
{
    public partial class SubLessonDetailsViewModel : BaseViewModel
    {
        private readonly SubLessonService _subLessonService;
        private SubLessonDetailsDto? _subLessonDetails;
        private MoveDto? _selectedMove;

        public ICommand BackToLessonsCommand { get; }

        public SubLessonDetailsDto? SubLessonDetails
        {
            get => _subLessonDetails;
            set
            {
                _subLessonDetails = value;
                OnPropertyChanged(nameof(SubLessonDetails));
            }
        }

        public MoveDto? SelectedMove
        {
            get => _selectedMove;
            set
            {
                _selectedMove = value;
                OnPropertyChanged(nameof(SelectedMove));
            }
        }

        public SubLessonDetailsViewModel(int subLessonId)
        {
            _subLessonService = new SubLessonService();
            BackToLessonsCommand = new Command(OnBackToLessons); // Initialize the command
            SubLessonDetails = new SubLessonDetailsDto(); // Default value to avoid null bindings
            _ = LoadSubLessonDetailsAsync(subLessonId);
        }

        private void OnBackToLessons()
        {
            // Logic for navigating back to lessons
        }

        public async Task LoadSubLessonDetailsAsync(int subLessonId)
        {
            try
            {
                Debug.WriteLine($"Start loading SubLesson ID: {subLessonId}");
                SubLessonDetails = await _subLessonService.GetSubLessonDetailsByIdAsync(subLessonId);

                if (SubLessonDetails == null)
                {
                    Debug.WriteLine("SubLessonDetails is null.");
                    throw new Exception("SubLessonDetails could not be fetched.");
                }

                Debug.WriteLine($"SubLesson Title: {SubLessonDetails.Title}");
                if (SubLessonDetails.Moves == null || SubLessonDetails.Moves.Count == 0)
                {
                    Debug.WriteLine("No moves available.");
                    SubLessonDetails.Moves = [];
                }

                SelectedMove = SubLessonDetails.Moves.FirstOrDefault();
                Debug.WriteLine($"Selected Move: {SelectedMove?.Name ?? "None"}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to load sublesson details.", "OK");
                }
                else
                {
                    Debug.WriteLine("MainPage is null. Cannot display alert.");
                }
            }
        }
    }
}
