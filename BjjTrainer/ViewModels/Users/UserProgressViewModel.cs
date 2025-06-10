using BjjTrainer.Models.Moves;
using BjjTrainer.Services.Users;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BjjTrainer.ViewModels.Users
{
    public partial class UserProgressViewModel : BaseViewModel
    {
        private readonly UserProgressService _userProgressService;

        public ObservableCollection<MoveDto> MovesPerformed { get; set; } = new ObservableCollection<MoveDto>();

        private string _totalTrainingTime = string.Empty;
        public string TotalTrainingTime
        {
            get => _totalTrainingTime;
            set => SetProperty(ref _totalTrainingTime, value);
        }

        private string _totalRoundsRolled = string.Empty;
        public string TotalRoundsRolled
        {
            get => _totalRoundsRolled;
            set => SetProperty(ref _totalRoundsRolled, value);
        }

        private string _totalSubmissions = string.Empty;
        public string TotalSubmissions
        {
            get => _totalSubmissions;
            set => SetProperty(ref _totalSubmissions, value);
        }

        private string _totalTaps = string.Empty;
        public string TotalTaps
        {
            get => _totalTaps;
            set => SetProperty(ref _totalTaps, value);
        }

        public ICommand LoadProgressCommand { get; }

        public UserProgressViewModel()
        {
            _userProgressService = new UserProgressService();
            LoadProgressCommand = new Command(async () => await LoadProgressAsync());
        }

        private async Task LoadProgressAsync()
        {
            try
            {
                // Get UserId from Preferences
                var userId = Preferences.Get("UserId", string.Empty);

                if (string.IsNullOrEmpty(userId))
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "User not logged in.", "OK");
                    }
                    return;
                }

                // Fetch progress data
                var progress = await _userProgressService.GetUserProgressAsync();

                if (progress != null)
                {
                    TotalTrainingTime = $"{progress.TotalTrainingTime} mins";
                    TotalRoundsRolled = $"{progress.TotalRoundsRolled}";
                    TotalSubmissions = $"{progress.TotalSubmissions}";
                    TotalTaps = $"{progress.TotalTaps}";
                    MovesPerformed.Clear();
                    foreach (var move in progress.MovesPerformed)
                    {
                        MovesPerformed.Add(move);
                    }
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load progress: {ex.Message}", "OK");
                }
            }
        }
    }
}
