﻿using BjjTrainer.Models.DTO;
using BjjTrainer.Models.Move;
using BjjTrainer.Services.Moves;
using BjjTrainer.Services.TrainingGoals;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.TrainingGoals;

public class TrainingGoalViewModel : BaseViewModel
{
    private readonly TrainingGoalService _trainingGoalService;
    private readonly MoveService _moveService;

    public DateTime GoalDate { get; set; } = DateTime.Today;
    public string Notes { get; set; }
    public ObservableCollection<Move> Moves { get; set; } = new();

    public TrainingGoalViewModel()
    {
        _trainingGoalService = new TrainingGoalService();
        _moveService = new MoveService();

        LoadMovesAsync();
    }

    private async Task LoadMovesAsync()
    {
        IsBusy = true;

        try
        {
            var moves = await _moveService.GetAllMovesAsync();
            Moves.Clear();
            foreach (var move in moves)
            {
                Moves.Add(new Move { Id = move.Id, Name = move.Name, IsSelected = false });
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", $"Failed to load moves: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task<bool> SaveGoalAsync()
{
    if (IsBusy) return false;
    IsBusy = true;

    try
    {
        // Validate data
        if (string.IsNullOrWhiteSpace(Notes) || GoalDate == default)
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Please fill in all required fields.", "OK");
            return false;
        }

        var selectedMoveIds = Moves.Where(m => m.IsSelected).Select(m => m.Id).ToList();

        if (!selectedMoveIds.Any())
        {
            await Application.Current.MainPage.DisplayAlert("Validation Error", "Please select at least one move.", "OK");
            return false;
        }

            var utcGoalDate = DateTime.SpecifyKind(GoalDate, DateTimeKind.Utc);

            var dto = new CreateTrainingGoalDto
        {
            ApplicationUserId = Preferences.Get("UserId", string.Empty),
            GoalDate = utcGoalDate, 
            Notes = Notes,
            MoveIds = selectedMoveIds
        };

        var result = await _trainingGoalService.CreateTrainingGoalAsync(dto);
        return result;
    }
    catch (Exception ex)
    {
        await Application.Current.MainPage.DisplayAlert("Error", $"Failed to save training goal: {ex.Message}", "OK");
        return false;
    }
    finally
    {
        IsBusy = false;
    }
}

}
