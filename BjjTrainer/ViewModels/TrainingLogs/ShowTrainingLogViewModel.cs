using BjjTrainer.Models.Moves;
using BjjTrainer.Services.Trainings;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.TrainingLogs;

public partial class ShowTrainingLogViewModel : BaseViewModel
{
    private readonly TrainingService _trainingService;

    public int Id { get; set; }  // Add this property
    public DateTime Date { get; set; }
    public double TrainingTime { get; set; }
    public int RoundsRolled { get; set; }
    public int Submissions { get; set; }
    public int Taps { get; set; }
    public string Notes { get; set; } = string.Empty;
    public string SelfAssessment { get; set; } = string.Empty;
    public ObservableCollection<LogMoveDto> Moves { get; set; } = [];
    private bool isCoachLog;
    public bool IsCoachLog
    {
        get => isCoachLog;
        set
        {
            if (isCoachLog != value)
            {
                isCoachLog = value;
                OnPropertyChanged(nameof(IsCoachLog));
            }
        }
    }


    public Command NavigateBackCommand { get; }

    public ShowTrainingLogViewModel()
    {
        _trainingService = new TrainingService();
        NavigateBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
    }

    public async Task LoadLogDetailsAsync(int logId)
    {
        IsBusy = true;

        try
        {
            Console.WriteLine($"Fetching training log with ID: {logId}");

            var log = await _trainingService.GetTrainingLogByIdAsync(logId);

            if (log == null)
            {
                Console.WriteLine($"No training log found for ID: {logId}");
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Training log not found.", "OK");
                }
                else
                {
                    Console.WriteLine("Application.Current.MainPage is null. Cannot display alert.");
                }
                return;
            }

            Console.WriteLine($"Training log retrieved: {log.Notes}");

            // Set properties from fetched log
            Id = log.Id;  // Map from DTO
            Date = log.Date;
            TrainingTime = log.TrainingTime;
            RoundsRolled = log.RoundsRolled;
            Submissions = log.Submissions;
            Taps = log.Taps;
            Notes = log.Notes ?? string.Empty;
            SelfAssessment = log.SelfAssessment ?? string.Empty;
            IsCoachLog = log.IsCoachLog;  // Bind IsCoachLog to ViewModel

            // Clear and repopulate Moves with null check
            Moves.Clear();
            if (log.Moves != null)
            {
                foreach (var move in log.Moves)
                {
                    Moves.Add(move);
                }
            }

            // Notify UI to update
            OnPropertyChanged(nameof(Date));
            OnPropertyChanged(nameof(TrainingTime));
            OnPropertyChanged(nameof(RoundsRolled));
            OnPropertyChanged(nameof(Submissions));
            OnPropertyChanged(nameof(Taps));
            OnPropertyChanged(nameof(Notes));
            OnPropertyChanged(nameof(SelfAssessment));
            OnPropertyChanged(nameof(Moves));
            OnPropertyChanged(nameof(IsCoachLog));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading training log details: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
