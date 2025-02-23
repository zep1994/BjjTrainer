using BjjTrainer.Models.DTO.Moves;
using BjjTrainer.Services.TrainingGoals;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.TrainingGoals;

public class ShowTrainingGoalViewModel : BaseViewModel
{
    private readonly TrainingGoalService _trainingGoalService;

    public int GoalId { get; }
    public DateTime GoalDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public ObservableCollection<MoveDto> Moves { get; set; } = [];

    public ShowTrainingGoalViewModel(int goalId)
    {
        GoalId = goalId;
        _trainingGoalService = new TrainingGoalService();

        Task.Run(async () => await LoadGoalDetailsAsync());
    }

    public async Task LoadGoalDetailsAsync()
    {
        IsBusy = true;

        try
        {
            var goal = await _trainingGoalService.GetTrainingGoalByIdAsync(GoalId);
            if (goal == null) throw new Exception("Training goal not found.");

            GoalDate = goal.GoalDate;
            Notes = goal.Notes;

            Moves.Clear();
            foreach (var move in goal.Moves)
            {
                Moves.Add(move);
            }

            OnPropertyChanged(nameof(GoalDate));
            OnPropertyChanged(nameof(Notes));
            OnPropertyChanged(nameof(Moves));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading training goal details: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
