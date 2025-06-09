using System.Collections.ObjectModel;
using BjjTrainer.Models.DTO.TrainingGoals;
using BjjTrainer.Models.Moves;
using BjjTrainer.Services.Moves;
using BjjTrainer.Services.TrainingGoals;
using MvvmHelpers;

namespace BjjTrainer.ViewModels.TrainingGoals;

public partial class UpdateTrainingGoalViewModel : BaseViewModel
{
    private readonly TrainingGoalService _trainingGoalService;
    private readonly MoveService _moveService;
    private readonly int _goalId;

    public DateTime GoalDate { get; set; }
    public string Notes { get; set; } = string.Empty;
    public ObservableCollection<Move> Moves { get; set; } = [];

    public UpdateTrainingGoalViewModel(int goalId)
    {
        _goalId = goalId;
        _trainingGoalService = new TrainingGoalService();
        _moveService = new MoveService();

        Task.Run(async () => await LoadGoalDetailsAsync());
    }

    public async Task LoadGoalDetailsAsync()
    {
        IsBusy = true;

        try
        {
            var goal = await _trainingGoalService.GetTrainingGoalByIdAsync(_goalId);
            if (goal != null)
            {
                GoalDate = goal.GoalDate;
                Notes = goal.Notes;

                var allMoves = await _moveService.GetAllMovesAsync();
                Moves.Clear();

                foreach (var move in allMoves)
                {
                    move.IsSelected = goal.Moves.Select(m => m.MoveId).Contains(move.Id);
                    Moves.Add(move);
                }

                OnPropertyChanged(nameof(GoalDate));
                OnPropertyChanged(nameof(Notes));
                OnPropertyChanged(nameof(Moves));
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading goal details: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task<bool> SaveGoalAsync()
    {
        IsBusy = true;

        try
        {
            var selectedMoveIds = Moves.Where(m => m.IsSelected).Select(m => m.Id).ToList();

            var dto = new CreateTrainingGoalDto
            {
                GoalDate = GoalDate,
                Notes = Notes,
                MoveIds = selectedMoveIds
            };

            await _trainingGoalService.UpdateTrainingGoalAsync(_goalId, dto);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating goal: {ex.Message}");
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }
}
