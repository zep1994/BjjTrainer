using BjjTrainer.ViewModels.Events;

namespace BjjTrainer.Views.Events;

public partial class ShowEventPage : ContentPage
{
    private readonly ShowEventViewModel _viewModel;

    public ShowEventPage(int eventId)
    {
        InitializeComponent();
        Console.WriteLine($"Navigating to ShowEventPage with EventId: {eventId}");
        _viewModel = new ShowEventViewModel(eventId);
        BindingContext = _viewModel;
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnUpdateButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new UpdateEventPage(_viewModel.EventId));
    }

    private async void OnCheckInButtonClicked(object sender, EventArgs e)
    {
        if (Application.Current?.MainPage != null) // Null-check added to avoid CS8602
        {
            if (await _viewModel.CheckInToEventAsync())
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Check-in successful!", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Check-in failed.", "OK");
            }
        }
        else
        {
            Console.WriteLine("Application.Current or MainPage is null.");
        }
    }
}
