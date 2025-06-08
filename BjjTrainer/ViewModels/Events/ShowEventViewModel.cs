using BjjTrainer.Models.DTO.TrainingLog;
using BjjTrainer.Services.Events;
using BjjTrainer.Services.Trainings;
using MvvmHelpers;

namespace BjjTrainer.ViewModels.Events
{
    public partial class ShowEventViewModel : BaseViewModel
    {
        private readonly EventService _eventService;
        public int EventId { get; }

        // Properties for Event Details
        public string Title { get; set; }
        public string Description { get; set; }
        public string FormattedDate { get; set; }
        public string FormattedEndDate { get; set; }
        public string FormattedStartTime { get; set; }
        public string FormattedEndTime { get; set; }
        public string TrainingLogId { get; set; }
        public TrainingLogDto? StudentTrainingLog { get; set; }

        public bool IsAllDay { get; set; }

        public ShowEventViewModel(int eventId)
        {
            EventId = eventId;
            _eventService = new EventService();
            Task.Run(async () => await LoadEventDetailsAsync());
        }

        public async Task LoadEventDetailsAsync()
        {
            try
            {
                var eventDetails = await _eventService.GetEventByIdAsync(EventId);

                if (eventDetails != null)
                {
                    Console.WriteLine($"Loaded Event Details: {eventDetails.Title}");

                    Title = eventDetails.Title;
                    Description = eventDetails.Description;
                    FormattedDate = eventDetails.StartDate?.ToString("MMMM dd, yyyy") ?? string.Empty;
                    FormattedEndDate = eventDetails.EndDate?.ToString("MMMM dd, yyyy") ?? string.Empty;

                    FormattedStartTime = eventDetails.StartTime.HasValue
                        ? eventDetails.StartTime.Value.ToString(@"hh\:mm")
                        : "00:00";

                    FormattedEndTime = eventDetails.EndTime.HasValue
                        ? eventDetails.EndTime.Value.ToString(@"hh\:mm")
                        : "00:00";

                    OnPropertyChanged(nameof(Title));
                    OnPropertyChanged(nameof(Description));
                    OnPropertyChanged(nameof(FormattedDate));
                    OnPropertyChanged(nameof(FormattedEndDate));
                    OnPropertyChanged(nameof(FormattedStartTime));
                    OnPropertyChanged(nameof(FormattedEndTime));
                }
                else
                {
                    Console.WriteLine("Event not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading event details: {ex.Message}");
            }
        }

        public async Task<bool> CheckInToEventAsync()
        {
            try
            {
                var success = await _eventService.CheckInToEventAsync(EventId);
                return success;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CheckInAndLoadLogAsync()
        {
            var logId = await _eventService.CheckInToEventAsync(EventId);
            if (logId)
            {
                // Fetch the log details
                var trainingService = new TrainingService();
                StudentTrainingLog = await trainingService.GetTrainingLogByIdAsync(EventId); // Assuming EventId is used to fetch the log
                OnPropertyChanged(nameof(StudentTrainingLog));
                return true;
            }
            return false;
        }
    }
}
