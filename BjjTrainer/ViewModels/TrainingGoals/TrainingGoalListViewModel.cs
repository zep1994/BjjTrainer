using BjjTrainer.Models.TrainingGoal;
using BjjTrainer.Services.TrainingGoals;
using BjjTrainer.Views.TrainingGoals;
using MvvmHelpers;
using System.Collections.ObjectModel;
using Command = Microsoft.Maui.Controls.Command; 

namespace BjjTrainer.ViewModels.TrainingGoals;

public partial class TrainingGoalListViewModel : BaseViewModel
{
    private readonly TrainingGoalService _trainingGoalService;
    private INavigation _navigation;

    public ObservableCollection<TrainingGoal> TrainingGoals { get; set; } = [];

    public Command AddNewGoalCommand { get; }
    public Command<int> DeleteGoalCommand { get; }
    public Command<int> ViewGoalCommand { get; }

    public TrainingGoalListViewModel(INavigation navigation)
    {
        _navigation = navigation;
        _trainingGoalService = new TrainingGoalService();
        AddNewGoalCommand = new Command(OnAddNewGoal);
        DeleteGoalCommand = new Command<int>(async (id) => await DeleteGoalAsync(id));
        ViewGoalCommand = new Command<int>(OnViewGoal);

        Task.Run(async () => await LoadGoalsAsync());
    }

    public async Task LoadGoalsAsync()
    {
        IsBusy = true;
        try
        {
            var goals = await _trainingGoalService.GetTrainingGoalsAsync();
            TrainingGoals.Clear();
            foreach (var goal in goals)
                TrainingGoals.Add(goal);
        }
        finally { IsBusy = false; }
    }

    public async Task DeleteGoalAsync(int goalId)
    {
        IsBusy = true;
        try
        {
            var success = await _trainingGoalService.DeleteTrainingGoalAsync(goalId);
            if (success)
            {
                var goalToRemove = TrainingGoals.FirstOrDefault(g => g.Id == goalId);
                if (goalToRemove != null)
                    TrainingGoals.Remove(goalToRemove);
            }
        }
        finally { IsBusy = false; }
    }

    private async void OnAddNewGoal()
    {
        await _navigation.PushAsync(new TrainingGoalPage());
    }

    private async void OnViewGoal(int goalId)
    {
        await _navigation.PushAsync(new ShowTrainingGoalPage(goalId));
    }
}
