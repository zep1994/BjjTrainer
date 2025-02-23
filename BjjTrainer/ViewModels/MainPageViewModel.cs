using BjjTrainer.Models.DTO.Moves;
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
    public class MainPageViewModel : BaseViewModel
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

        public MainPageViewModel()
        {
            _trainingService = new TrainingService();
            _eventService = new EventService();
            _userProgressService = new UserProgressService();
            _moveService = new MoveService();
            _trainingGoalService = new TrainingGoalService(); 

            LoadData();
        }

        private async void LoadData()
        {
            try
            {
                IsBusy = true;
                await LoadUserProgressAsync();
                await LoadGoalsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading data: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task LoadUserProgressAsync()
        {
            try
            {
                var userProgress = await _userProgressService.GetUserProgressAsync();

                // Assign progress data
                TotalTrainingTime = userProgress.TotalTrainingTime;
                TotalRoundsRolled = userProgress.TotalRoundsRolled;
                TotalSubmissions = userProgress.TotalSubmissions;
                TotalTaps = userProgress.TotalTaps;
                WeeklyTrainingHours = userProgress.WeeklyTrainingHours;
                AverageSessionLength = userProgress.AverageSessionLength;
                FavoriteMoveThisMonth = userProgress.FavoriteMoveThisMonth;
                TotalGoalsAchieved = userProgress.TotalGoalsAchieved;
                TotalMoves = userProgress.TotalMoves;

                // Clear existing data
                MovesPerformed.Clear();

                // Add fetched moves to the collection
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

                Console.WriteLine($"Goals Count: {goals?.Count()}");

                TrainingGoals.Clear();

                foreach (var goal in goals)
                {
                    TrainingGoals.Add(goal);
                }

                OnPropertyChanged(nameof(TrainingGoals));
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



        //private async void LoadTopMoves()
        //{
        //    try
        //    {
        //        var topMoves = _moveService.GetAllMovesAsync();
        //        Console.WriteLine($"Top Moves: {topMoves}");

        //        Tp
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error: {ex.Message}");
        //    }
        //}
    }
}
