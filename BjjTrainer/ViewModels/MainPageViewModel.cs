using BjjTrainer.Models.DTO.Users;
using BjjTrainer.Models.Moves;
using BjjTrainer.Models.TrainingGoal;
using BjjTrainer.Services.Events;
using BjjTrainer.Services.Moves;
using BjjTrainer.Services.TrainingGoals;
using BjjTrainer.Services.Trainings;
using BjjTrainer.Services.Users;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input; 

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
        public ObservableCollection<GraphViewModel> Graphs { get; } = [];

        // Properties for User Progress
        public double? TotalTrainingTime { get; private set; }
        public int? TotalRoundsRolled { get; private set; }
        public int? TotalSubmissions { get; private set; }
        public int? TotalTaps { get; private set; }
        public double? WeeklyTrainingHours { get; private set; }
        public double? AverageSessionLength { get; private set; }
        public string? FavoriteMoveThisMonth { get; private set; }
        public int? TotalGoalsAchieved { get; private set; }
        public int? TotalMoves { get; private set; }

        public string? UserName { get; private set; }
        public string? Email { get; private set; }
        public string? Belt { get; private set; }
        public int? BeltStripes { get; private set; }
        public string? ProfilePictureUrl { get; private set; }
        public string? PreferredTrainingStyle { get; private set; }

        public ObservableCollection<DailyUserProgressDto> DailyTrainingTime { get; } = [];
        public ObservableCollection<DailyUserProgressDto> DailyRoundsRolled { get; } = [];
        public ObservableCollection<DailyUserProgressDto> DailySubmissions { get; } = [];
        public ObservableCollection<DailyUserProgressDto> DailyTaps { get; } = [];

        public double WeeklyTrainingTimeSum => DailyTrainingTime.Sum(x => x.Value);
        public double WeeklyRoundsRolledSum => DailyRoundsRolled.Sum(x => x.Value);
        public double WeeklySubmissionsSum => DailySubmissions.Sum(x => x.Value);
        public double WeeklyTapsSum => DailyTaps.Sum(x => x.Value);

        public double MostTrainedTimeOneDayThisMonth =>
            DailyTrainingTime
                .Where(x => x.Date.Month == DateTime.Now.Month && x.Date.Year == DateTime.Now.Year)
                .OrderByDescending(x => x.Value)
                .FirstOrDefault()?.Value ?? 0;

        private int _currentGraphIndex;
        public int CurrentGraphIndex
        {
            get => _currentGraphIndex;
            set
            {
                if (SetProperty(ref _currentGraphIndex, value))
                    OnPropertyChanged(nameof(CurrentGraphTitle));
            }
        }

        public string CurrentGraphTitle => Graphs.Count > 0 && CurrentGraphIndex >= 0 && CurrentGraphIndex < Graphs.Count
            ? Graphs[CurrentGraphIndex].Title ?? string.Empty
            : string.Empty;

        public ICommand NextGraphCommand { get; }
        public ICommand PreviousGraphCommand { get; }

        public MainPageViewModel()
        {
            _trainingService = new TrainingService();
            _eventService = new EventService();
            _userProgressService = new UserProgressService();
            _moveService = new MoveService();
            _trainingGoalService = new TrainingGoalService();

            NextGraphCommand = new RelayCommand(NextGraph, CanGoNext);
            PreviousGraphCommand = new RelayCommand(PreviousGraph, CanGoPrevious);
        }

        public async Task InitializeAsync()
        {
            IsBusy = true;
            try
            {
                await LoadUserInfoAsync();
                await LoadUserProgressAsync();
                await LoadGoalsAsync();
                await LoadWeeklyGraphsAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadWeeklyGraphsAsync()
        {
            var trainingTime = await _userProgressService.GetDailyTotalTrainingTimeLast7DaysAsync();
            var roundsRolled = await _userProgressService.GetDailyTotalRoundsRolledLast7DaysAsync();
            var submissions = await _userProgressService.GetDailyTotalSubmissionsLast7DaysAsync();
            var taps = await _userProgressService.GetDailyTotalTapsLast7DaysAsync();

            Graphs.Clear();
            Graphs.Add(new GraphViewModel { Title = "Training Time (min)", Data = new ObservableCollection<DailyUserProgressDto>(trainingTime) });
            Graphs.Add(new GraphViewModel { Title = "Rounds Rolled", Data = new ObservableCollection<DailyUserProgressDto>(roundsRolled) });
            Graphs.Add(new GraphViewModel { Title = "Submissions", Data = new ObservableCollection<DailyUserProgressDto>(submissions) });
            Graphs.Add(new GraphViewModel { Title = "Taps", Data = new ObservableCollection<DailyUserProgressDto>(taps) });

            OnPropertyChanged(nameof(Graphs));
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
                        UserName = user.UserName ?? string.Empty;
                        Email = user.Email ?? string.Empty;
                        Belt = user.Belt ?? string.Empty;
                        BeltStripes = user.BeltStripes;
                        ProfilePictureUrl = user.ProfilePictureUrl ?? string.Empty;
                        PreferredTrainingStyle = user.PreferredTrainingStyle ?? string.Empty;

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
                FavoriteMoveThisMonth = userProgress.FavoriteMoveThisMonth ?? string.Empty; 
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
                    Console.WriteLine($"Goals Count: {goals.Count}"); 

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

        private void NextGraph()
        {
            if (CurrentGraphIndex < Graphs.Count - 1)
                CurrentGraphIndex++;
        }

        private void PreviousGraph()
        {
            if (CurrentGraphIndex > 0)
                CurrentGraphIndex--;
        }

        private bool CanGoNext() => CurrentGraphIndex < Graphs.Count - 1;
        private bool CanGoPrevious() => CurrentGraphIndex > 0;

        private static List<DailyUserProgressDto> GenerateLast7DaysData(Func<DateTime, double> valueSelector)
        {
            var today = DateTime.Today;
            var data = new List<DailyUserProgressDto>();
            // Oldest (6 days ago) to newest (today)
            for (int i = 6; i >= 0; i--)
            {
                var date = today.AddDays(-i);
                data.Add(new DailyUserProgressDto
                {
                    Date = date,
                    Value = valueSelector(date)
                });
            }
            return data;
        }
    }
}
