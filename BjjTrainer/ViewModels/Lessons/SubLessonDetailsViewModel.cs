using BjjTrainer.Models.DTO;
using BjjTrainer.Models.DTO.Moves;
using BjjTrainer.Services.Lessons;
using MvvmHelpers;
using System.Diagnostics;
using System.Windows.Input;

namespace BjjTrainer.ViewModels
{
    public partial class SubLessonDetailsViewModel : BaseViewModel
    {
        private readonly SubLessonService _subLessonService;
        private SubLessonDetailsDto _subLessonDetails;
        private MoveDto _selectedMove;


        public ICommand BackToLessonsCommand { get; }


        public SubLessonDetailsDto SubLessonDetails
        {
            get => _subLessonDetails;
            set
            {
                _subLessonDetails = value;
                OnPropertyChanged(nameof(SubLessonDetails)); // Trigger UI update
            }
        }

        public MoveDto SelectedMove
        {
            get => _selectedMove;
            set
            {
                _selectedMove = value;
                OnPropertyChanged(nameof(SelectedMove)); // Trigger UI update
            }
        }

        public SubLessonDetailsViewModel(int subLessonId)
        {
            _subLessonService = new SubLessonService();
            SubLessonDetails = new SubLessonDetailsDto(); // Default value to avoid null bindings
            BackToLessonsCommand = new Command(OnBackToLessons);
            _ = LoadSubLessonDetailsAsync(subLessonId);
        }

        // File: SubLessonDetailsViewModel.cs
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
                if (SubLessonDetails.Moves == null || !SubLessonDetails.Moves.Any())
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
                await Application.Current.MainPage.DisplayAlert("Error", "Failed to load sublesson details.", "OK");
            }
        }



        private async void OnBackToLessons()
        {
            // Logic for navigating back to the lessons page (can use Navigation.PopAsync() or other means depending on app structure)
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
