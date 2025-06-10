using BjjTrainer.ViewModels.Events;

namespace BjjTrainer.Views.Events;

public partial class UpdateEventPage : ContentPage
{
    private readonly UpdateEventViewModel _viewModel;

    public UpdateEventPage(int eventId)
    {
        InitializeComponent();
        Console.WriteLine($"Navigating to UpdateEventPage with EventId: {eventId}");
        _viewModel = new UpdateEventViewModel(eventId);
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadEventDetailsAsync();
    }

    private async void OnSaveEventClicked(object sender, EventArgs e)
    {
        var success = await _viewModel.SaveEventAsync();
        if (success)
        {
            await DisplayAlert("Success", "Event updated successfully.", "OK");
            await Navigation.PopAsync();
        }
        else
        {
            await DisplayAlert("Error", "Failed to update event.", "OK");
        }
    }

    public async void OnCalendarBackClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CalendarPage());
    }

    private async void OnDeleteEventClicked(object sender, EventArgs e)
    {
        var confirm = await DisplayAlert("Confirm", "Are you sure you want to delete this event?", "Yes", "No");
        if (confirm)
        {
            var success = await _viewModel.DeleteEventAsync();
            if (success)
            {
                await DisplayAlert("Success", "Event deleted successfully.", "OK");
                await Navigation.PopToRootAsync();
            }
            else
            {
                await DisplayAlert("Error", "Failed to delete event.", "OK");
            }
        }
    }
}
