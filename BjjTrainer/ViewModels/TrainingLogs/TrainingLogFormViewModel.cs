using BjjTrainer.Models.DTO.TrainingLog;
using BjjTrainer.Models.Moves;
using BjjTrainer.Services.Moves;
using BjjTrainer.Services.Trainings;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.TrainingLogs
{
    public partial class TrainingLogFormViewModel : BaseViewModel
    {
        private readonly TrainingService _trainingService;
        private readonly MoveService _moveService;

        public ObservableCollection<Move> Moves { get; private set; } = [];

        public DateTime Date { get; set; } = DateTime.Now;
        public double? TrainingTime { get; set; }
        public int? RoundsRolled { get; set; }
        public int? Submissions { get; set; }
        public int? Taps { get; set; }
        public string? Notes { get; set; }
        public string? SelfAssessment { get; set; }


        public TrainingLogFormViewModel()
        {
            _trainingService = new TrainingService();
            _moveService = new MoveService();
            LoadMovesAsync();
        }

        private async void LoadMovesAsync()
        {
            try
            {
                var moves = await _moveService.GetAllMovesAsync();
                Moves.Clear();
                foreach (var move in moves)
                {
                    move.IsSelected = false;
                    Moves.Add(move);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading moves: {ex.Message}");
            }
        }

        public async Task<bool> SubmitLogAsync()
        {
            IsBusy = true;

            try
            {
                string userId = Preferences.Get("UserId", string.Empty);

                if (string.IsNullOrEmpty(userId))
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "User not authenticated. Please log in.", "OK");
                    }
                    return false;
                }

                var selectedMoves = Moves.Where(m => m.IsSelected).Select(m => m.Id).ToList();
                if (!selectedMoves.Any())
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Are you sure you did not train any moves?", "OK");
                    }
                }

                var trainingLog = new CreateTrainingLogDto
                {
                    ApplicationUserId = userId,
                    Date = Date,
                    TrainingTime = TrainingTime ?? 0,
                    RoundsRolled = RoundsRolled ?? 0,
                    Submissions = Submissions ?? 0,
                    Taps = Taps ?? 0,
                    Notes = Notes ?? "",
                    SelfAssessment = SelfAssessment ?? string.Empty, // Fix for CS8601
                    MoveIds = selectedMoves ?? []
                };

                var success = await _trainingService.SubmitTrainingLogAsync(trainingLog);

                if (success)
                {
                    if (Application.Current?.MainPage != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Training log submitted successfully!", "OK");
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Failed to submit training log: {ex.Message}", "OK");
                }
            }
            finally
            {
                IsBusy = false;
            }

            return false;
        }
    }
}
