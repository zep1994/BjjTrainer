using BjjTrainer.ViewModels.Events;
using Syncfusion.Maui.Scheduler;

namespace BjjTrainer.Views.Events;

public partial class CalendarPage : ContentPage
{
    public CalendarPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CalendarViewModel vm)
        {
            await vm.LoadAppointments();
        }
        EventScheduler.View = SchedulerView.Week;
    }

    private async void OnSchedulerTapped(object sender, SchedulerTappedEventArgs e)
    {
        if (e.Appointments?.FirstOrDefault() is SchedulerAppointment appointment && appointment.Id != null)
        {
            Console.WriteLine($"Tapped appointment Id: {appointment.Id}, Type: {appointment.Id.GetType()}");
            int eventId = Convert.ToInt32(appointment.Id);
            Console.WriteLine($"Navigating to ShowEventPage with EventId: {eventId}");

            if (eventId > 0)
            {
                await Navigation.PushAsync(new ShowEventPage(eventId));
            }
            else
            {
                Console.WriteLine("Invalid eventId detected. Navigation canceled.");
            }
        }
        // Removed navigation to new CalendarPage to avoid navigation loop
    }

    // Navigate to CreateEventPage instead of triggering ShowAppointmentEditor
    private async void OnAddEventClicked(object sender, EventArgs e)
    {
        var now = DateTime.Now.Date;
        var startTime = now.AddHours(18);
        var endTime = startTime.AddHours(1);

        await Navigation.PushAsync(new CreateEventPage(startTime, endTime));
    }


    private async void OnAppointmentDrop(object sender, AppointmentDropEventArgs e)
    {
        if (e.Appointment is SchedulerAppointment appointment
            && BindingContext is CalendarViewModel vm)
        {
            await vm.UpdateDroppedEventAsync(appointment);
        }
    }
}
