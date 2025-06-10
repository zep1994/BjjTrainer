using BjjTrainer.Models.DTO.Events;
using BjjTrainer.Models.Moves;
using BjjTrainer.Services.Events;
using BjjTrainer.Services.Moves;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.Events
{
    public partial class CreateEventViewModel : BaseViewModel
    {
        private readonly EventService _eventService;
        private readonly MoveService _moveService;

        public new string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now.Date.AddDays(0);
        public TimeSpan StartTime { get; set; } = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1));
        public DateTime EndDate { get; set; } = DateTime.Now.Date.AddDays(1);
        public TimeSpan EndTime { get; set; } = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1));
        public bool IsAllDay { get; set; } = false;
        private bool _includeTrainingLog;
        public bool IncludeTrainingLog
        {
            get => _includeTrainingLog;
            set
            {
                if (_includeTrainingLog != value)
                {
                    _includeTrainingLog = value;
                    OnPropertyChanged();

                    // Dynamically load moves when checked, clear when unchecked
                    if (_includeTrainingLog)
                    {
                        LoadAvailableMoves();
                    }
                    else
                    {
                        AvailableMoves.Clear();
                    }
                }
            }
        }

        public ObservableCollection<Move> AvailableMoves { get; set; } = [];
        public ObservableCollection<Move> SelectedMoves { get; set; } = [];

        public CreateEventViewModel()
        {
            var now = DateTime.Now.Date;
            StartDate = now;
            StartTime = new TimeSpan(18, 0, 0);  // Default to 6:00 PM
            EndDate = now;
            EndTime = new TimeSpan(19, 0, 0);  // Default to 7:00 PM
            _eventService = new EventService();
            _moveService = new MoveService();
            LoadAvailableMoves();
        }

        // Fetch available moves for selection
        private async void LoadAvailableMoves()
        {
            try
            {
                var moves = await _moveService.GetAllMovesAsync();
                AvailableMoves.Clear();

                foreach (var move in moves)
                {
                    AvailableMoves.Add(move);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading moves: {ex.Message}");
            }
        }

        public async Task<bool> SaveEventAsync(CalendarEventCreateDTO dto)
        {
            // Ensure required fields are set
            if (string.IsNullOrWhiteSpace(dto.Title) || dto.StartDate == default || dto.EndDate == default)
                return false;

            // Set ApplicationUserId from preferences
            dto.ApplicationUserId = Preferences.Get("UserId", string.Empty);

            // Defensive: Ensure MoveIds is not null
            dto.MoveIds ??= [];

            // Call the API
            try
            {
                return await _eventService.CreateEventAsync(dto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving event: {ex.Message}");
                return false;
            }
        }
    }
}
