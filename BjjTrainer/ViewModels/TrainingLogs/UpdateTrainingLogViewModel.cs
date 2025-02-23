using BjjTrainer.Messages;
using BjjTrainer.Models.DTO.Moves;
using BjjTrainer.Models.DTO.TrainingLog;
using BjjTrainer.Services.Trainings;
using BjjTrainer.Views.Components;
using CommunityToolkit.Mvvm.Messaging;
using MvvmHelpers;
using System.Collections.ObjectModel;

public partial class UpdateTrainingLogViewModel : ObservableObject
{
    private readonly TrainingService _trainingService;

    public ObservableCollection<UpdateMoveDto> Moves { get; set; } = [];

    private DateTime _date;
    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    private double _trainingTime;
    public double TrainingTime
    {
        get => _trainingTime;
        set => SetProperty(ref _trainingTime, value);
    }

    private int _roundsRolled;
    public int RoundsRolled
    {
        get => _roundsRolled;
        set => SetProperty(ref _roundsRolled, value);
    }

    private int _submissions;
    public int Submissions
    {
        get => _submissions;
        set => SetProperty(ref _submissions, value);
    }

    private int _taps;
    public int Taps
    {
        get => _taps;
        set => SetProperty(ref _taps, value);
    }

    private string? _notes;
    public string? Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    private string? _selfAssessment;
    public string? SelfAssessment
    {
        get => _selfAssessment;
        set => SetProperty(ref _selfAssessment, value);
    }

    private bool _isCoachLog;
    public bool IsCoachLog
    {
        get => _isCoachLog;
        set => SetProperty(ref _isCoachLog, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public int LogId { get; set; }

    public UpdateTrainingLogViewModel(int logId)
    {
        LogId = logId;
        _trainingService = new TrainingService();

        // Register for messenger updates
        WeakReferenceMessenger.Default.Register<SelectedMovesUpdatedMessage>(this, (r, m) =>
        {
            if (m.Moves?.Any() == true)
            {
                Moves.Clear();
                foreach (var move in m.Moves)
                {
                    Moves.Add(move);
                }
            }
        });
    }

    public void Initialize(int logId)
    {
        LogId = logId;
        Task.Run(async () => await LoadTrainingLogDetailsAsync());
    }

    public async Task LoadTrainingLogDetailsAsync()
    {
        try
        {
            var log = await _trainingService.GetTrainingLogMoves(LogId);
            if (log != null)
            {
                Date = log.Date;
                TrainingTime = log.TrainingTime;
                RoundsRolled = log.RoundsRolled;
                Submissions = log.Submissions;
                Taps = log.Taps;
                Notes = log.Notes;
                SelfAssessment = log.SelfAssessment;
                IsCoachLog = log.IsCoachLog;

                Moves.Clear();
                foreach (var move in log.Moves)
                {
                    Moves.Add(move);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading log details: {ex.Message}");
            await SafeDisplayAlert("Error", $"Failed to load training log: {ex.Message}", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    public async Task EditMovesAsync()
    {
        var modal = new MoveSelectionModal(new ObservableCollection<UpdateMoveDto>(Moves), LogId);
        await Application.Current?.MainPage?.Navigation.PushModalAsync(modal);
    }

    public async Task<bool> UpdateLogAsync()
    {
        IsBusy = true;

        try
        {
            var updatedLog = new UpdateTrainingLogDto
            {
                Date = Date,
                TrainingTime = TrainingTime,
                RoundsRolled = RoundsRolled,
                Submissions = Submissions,
                Taps = Taps,
                Notes = Notes,
                SelfAssessment = SelfAssessment,
                Moves = new ObservableCollection<UpdateMoveDto>(Moves)
            };

            await _trainingService.UpdateTrainingLogAsync(LogId, updatedLog, IsCoachLog);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating log: {ex.Message}");
            return false;
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SafeDisplayAlert(string title, string message, string cancel)
    {
        if (Application.Current?.MainPage != null)
        {
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
        else
        {
            Console.WriteLine($"Alert: {title} - {message}");
        }
    }
}
