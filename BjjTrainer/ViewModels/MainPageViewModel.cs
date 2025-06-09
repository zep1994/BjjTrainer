using BjjTrainer.Models.Moves;
using BjjTrainer.Models.TrainingGoal;
using BjjTrainer.Services.Events;
using BjjTrainer.Services.Moves;
using BjjTrainer.Services.TrainingGoals;
using BjjTrainer.Services.Trainings;
using BjjTrainer.Services.Users;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels
{
    public partial class MainPageViewModel : BaseViewModel
    {
        private readonly TrainingService _trainingService;
        private readonly EventService _eventService;
        private readonly UserProgressService _userProgressService;
        private readonly MoveService _moveService;
        private readonly TrainingGoalService _trainingGoalService;

        public ObservableCollection<MoveDto> MovesPerformed { get; set; } = [];
        public ObservableCollection<TrainingGoal> TrainingGoals { get; set; } = [];

        // Properties for User Progress
        public double TotalTrainingTime { get; private set; }
        public int TotalRoundsRolled { get; private set; }
        public int TotalSubmissions { get; private set; }
        public int TotalTaps { get; private set; }
        public double WeeklyTrainingHours { get; private set; }
        public double AverageSessionLength { get; private set; }
        public string FavoriteMoveThisMonth { get; private set; }
        public int TotalGoalsAchieved { get; private set; }
        public int TotalMoves { get; private set; }

        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string Belt { get; private set; }
        public int BeltStripes { get; private set; }
        public string ProfilePictureUrl { get; private set; }
        public string PreferredTrainingStyle { get; private set; }

        public MainPageViewModel()
        {
            _trainingService = new TrainingService();
            _eventService = new EventService();
            _userProgressService = new UserProgressService();
            _moveService = new MoveService();
            _trainingGoalService = new TrainingGoalService();
        }

        public async Task InitializeAsync()
        {
            IsBusy = true;
            try
            {
                await LoadUserInfoAsync();
                await LoadUserProgressAsync();
                await LoadGoalsAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadUserInfoAsync()
        {
            try
            {
                var userId = Preferences.Get("UserId", string.Empty);
                if (!string.IsNullOrEmpty(userId))
                {
                    var userService = new UserService();
                    var user = await userService.GetUserByIdAsync(userId);
                    if (user != null)
                    {
                        UserName = user.UserName;
                        Email = user.Email;
                        Belt = user.Belt;
                        BeltStripes = user.BeltStripes;
                        ProfilePictureUrl = user.ProfilePictureUrl;
                        PreferredTrainingStyle = user.PreferredTrainingStyle;

                        OnPropertyChanged(nameof(UserName));
                        OnPropertyChanged(nameof(Email));
                        OnPropertyChanged(nameof(Belt));
                        OnPropertyChanged(nameof(BeltStripes));
                        OnPropertyChanged(nameof(ProfilePictureUrl));
                        OnPropertyChanged(nameof(PreferredTrainingStyle));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user info: {ex.Message}");
            }
        }

        private async Task LoadUserProgressAsync()
        {
            try
            {
                var userProgress = await _userProgressService.GetUserProgressAsync();

                TotalTrainingTime = userProgress.TotalTrainingTime;
                TotalRoundsRolled = userProgress.TotalRoundsRolled;
                TotalSubmissions = userProgress.TotalSubmissions;
                TotalTaps = userProgress.TotalTaps;
                WeeklyTrainingHours = userProgress.WeeklyTrainingHours;
                AverageSessionLength = userProgress.AverageSessionLength;
                FavoriteMoveThisMonth = userProgress.FavoriteMoveThisMonth;
                TotalGoalsAchieved = userProgress.TotalGoalsAchieved;
                TotalMoves = userProgress.TotalMoves;

                MovesPerformed.Clear();

                foreach (var move in userProgress.MovesPerformed)
                {
                    MovesPerformed.Add(move);
                }

                OnPropertyChanged(nameof(TotalTrainingTime));
                OnPropertyChanged(nameof(TotalRoundsRolled));
                OnPropertyChanged(nameof(TotalSubmissions));
                OnPropertyChanged(nameof(TotalTaps));
                OnPropertyChanged(nameof(WeeklyTrainingHours));
                OnPropertyChanged(nameof(AverageSessionLength));
                OnPropertyChanged(nameof(FavoriteMoveThisMonth));
                OnPropertyChanged(nameof(TotalGoalsAchieved));
                OnPropertyChanged(nameof(TotalMoves));
                OnPropertyChanged(nameof(MovesPerformed));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user progress: {ex.Message}");
            }
        }

        private async Task LoadGoalsAsync()
        {
            try
            {
                IsBusy = true;
                var goals = await _trainingGoalService.GetTrainingGoalsAsync();

                if (goals != null)
                {
                    Console.WriteLine($"Goals Count: {goals.Count()}");

                    TrainingGoals.Clear();

                    foreach (var goal in goals)
                    {
                        TrainingGoals.Add(goal);
                    }

                    OnPropertyChanged(nameof(TrainingGoals));
                }
                else
                {
                    Console.WriteLine("Goals list is null.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading training goals: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
