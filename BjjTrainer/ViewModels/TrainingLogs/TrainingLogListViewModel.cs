using BjjTrainer.Models.DTO.TrainingLog;
using BjjTrainer.Services.Trainings;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.TrainingLogs
{
    public partial class TrainingLogListViewModel : BaseViewModel
    {
        private readonly TrainingService _trainingService;
        private readonly INavigation? _navigation; 

        public ObservableCollection<TrainingLogDto> TrainingLogs { get; set; } = [];

        // Public Parameterless Constructor
        public TrainingLogListViewModel()
        {
            _trainingService = new TrainingService();
            LoadTrainingLogs();
        }

        public TrainingLogListViewModel(INavigation navigation)
        {
            _trainingService = new TrainingService();
            _navigation = navigation;
            LoadTrainingLogs();
        }

        public async void LoadTrainingLogs()
        {
            IsBusy = true;

            try
            {
                var userId = Preferences.Get("UserId", string.Empty);
                if (!string.IsNullOrEmpty(userId))
                {
                    var logs = await _trainingService.GetTrainingLogsAsync(userId);
                    TrainingLogs.Clear();

                    foreach (var log in logs)
                    {
                        TrainingLogs.Add(log);
                        Console.WriteLine($"Loaded Log: {log.Date.ToShortDateString()} - {log.TrainingTime} hrs");
                    }
                }
                else
                {
                    Console.WriteLine("User ID not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading training logs: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}