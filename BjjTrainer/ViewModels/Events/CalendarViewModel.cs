using System.Collections.ObjectModel;
using BjjTrainer.Models.DTO.Events;
using BjjTrainer.Services.Events;
using MvvmHelpers;
using Syncfusion.Maui.Scheduler;

namespace BjjTrainer.ViewModels.Events
{
    public partial class CalendarViewModel : BaseViewModel
    {
        private readonly EventService _eventService;

        private ObservableCollection<SchedulerAppointment> _appointments = [];
        public ObservableCollection<SchedulerAppointment> Appointments
        {
            get => _appointments;
            set => SetProperty(ref _appointments, value);
        }

        public CalendarViewModel()
        {
            _eventService = new EventService();
        }

        // LOAD EVENT
        public async Task LoadAppointments()
        {
            var userId = Preferences.Get("UserId", string.Empty);

            var events = await _eventService.GetAllUserEventsAsync(userId);

            // Log the number of events fetched
            System.Diagnostics.Debug.WriteLine($"Fetched {events?.Count ?? 0} events for user {userId}");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                Appointments.Clear();
                if (events != null)
                {
                    foreach (var calendarEvent in events)
                    {
                        try
                        {
                            // Fix: Check for StartDate != default instead of HasValue
                            if (!calendarEvent.StartDate.HasValue || calendarEvent.StartDate.Value == default)
                            {
                                System.Diagnostics.Debug.WriteLine($"Skipping event {calendarEvent.Id} due to missing StartDate.");
                                continue;
                            }
                            var startDate = calendarEvent.StartDate.Value.Date;
                            var endDate = calendarEvent.EndDate?.Date ?? startDate;

                            var startTime = TimeSpan.Zero;
                            if (calendarEvent.StartTime.HasValue && !string.IsNullOrWhiteSpace(calendarEvent.StartTime.ToString()))
                                TimeSpan.TryParse(calendarEvent.StartTime.ToString(), out startTime);

                            var endTime = TimeSpan.Zero;
                            if (calendarEvent.EndTime.HasValue && !string.IsNullOrWhiteSpace(calendarEvent.EndTime.ToString()))
                                TimeSpan.TryParse(calendarEvent.EndTime.ToString(), out endTime);

                            var startDateTime = startDate.Add(startTime);
                            var endDateTime = endDate.Add(endTime);

                            System.Diagnostics.Debug.WriteLine(
                                $"Adding appointment: Id={calendarEvent.Id}, Title={calendarEvent.Title}, Start={startDateTime}, End={endDateTime}");

                            var appointment = new SchedulerAppointment
                            {
                                Id = calendarEvent.Id, // Make sure this is always an int and not null
                                Subject = calendarEvent.Title ?? "Event",
                                Notes = calendarEvent.Description ?? "",
                                Background = Colors.BlueViolet,
                                IsAllDay = calendarEvent.IsAllDay,
                                StartTime = DateTime.SpecifyKind(startDateTime, DateTimeKind.Local),
                                EndTime = DateTime.SpecifyKind(endDateTime, DateTimeKind.Local),
                            };
                            System.Diagnostics.Debug.WriteLine($"SchedulerAppointment created: Id={appointment.Id}, Type={appointment.Id?.GetType()}");

                            if (appointment.StartTime == appointment.EndTime)
                            {
                                appointment.EndTime = appointment.StartTime.Add(TimeSpan.FromMinutes(1));
                            }

                            Appointments.Add(appointment);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error parsing event: {ex.Message}");
                        }
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No events returned from service.");
                }
                System.Diagnostics.Debug.WriteLine($"Total appointments in collection: {Appointments.Count}");
            });
        }

        public async Task UpdateDroppedEventAsync(SchedulerAppointment appointment)
        {
            var updatedEvent = new CalendarEventUpdateDto // Change from CalendarEventDto to CalendarEventUpdateDto
            {
                Id = appointment.Id != null ? Convert.ToInt32(appointment.Id) : 0,
                Title = appointment.Subject,
                Description = appointment.Notes,
                StartDate = appointment.StartTime,
                EndDate = appointment.EndTime,
                IsAllDay = appointment.IsAllDay,
                StartTime = appointment.StartTime.TimeOfDay,
                EndTime = appointment.EndTime.TimeOfDay
            };
            await _eventService.UpdateEventAsync(updatedEvent.Id, updatedEvent); // This now matches the expected type
            await LoadAppointments();
        }
    }
}
