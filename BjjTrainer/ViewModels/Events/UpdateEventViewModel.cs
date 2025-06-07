using BjjTrainer.Models.DTO.Events;
using BjjTrainer.Services.Events;
using MvvmHelpers;

namespace BjjTrainer.ViewModels.Events
{
    public partial class UpdateEventViewModel(int eventId) : BaseViewModel
    {
        private readonly EventService _eventService = new EventService();
        public int Id { get; } = eventId;
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool? IsAllDay { get; set; }
        public int? schoolId { get; set; }

        public async Task LoadEventDetailsAsync()
        {
            try
            {
                var eventDetails = await _eventService.GetEventByIdAsync(Id);
                Console.WriteLine($"Loaded Event for Update: {eventDetails.Title}");

                Title = eventDetails.Title;
                Description = eventDetails.Description;
                StartDate = eventDetails.StartDate ?? DateTime.MinValue;
                EndDate = eventDetails.EndDate ?? DateTime.MinValue;
                StartTime = eventDetails.StartTime ?? TimeSpan.Zero;
                EndTime = eventDetails.EndTime ?? TimeSpan.Zero;

                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Description));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(EndTime));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading event details for update: {ex.Message}");
            }
        }

        public async Task<bool> SaveEventAsync()
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(Title))
            {
                Console.WriteLine("Title is required.");
                return false;
            }
            if (StartDate == null || EndDate == null)
            {
                Console.WriteLine("Start and End dates are required.");
                return false;
            }
            if (StartTime == null || EndTime == null)
            {
                Console.WriteLine("Start and End times are required.");
                return false;
            }

            var schoolId = Preferences.Get("SchoolId", 1);

            var updatedEvent = new CalendarEventUpdateDto
            {
                Id = Id,
                Title = Title,
                Description = Description,
                StartDate = StartDate,
                StartTime = StartTime,
                EndDate = EndDate,
                EndTime = EndTime,
                IsAllDay = IsAllDay ?? false,
                SchoolId = schoolId,
            };

            // Log the DTO for debugging
            try
            {
                var json = System.Text.Json.JsonSerializer.Serialize(updatedEvent);
                Console.WriteLine($"Updating event with DTO: {json}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing DTO: {ex.Message}");
            }

            try
            {
                return await _eventService.UpdateEventAsync(Id, updatedEvent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating event: {ex.Message}");
                return false;
            }
        }
    }
}
