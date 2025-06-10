using BjjTrainer.ViewModels.TrainingLogs;

namespace BjjTrainer.Views.Training;

[QueryProperty(nameof(LogId), "logId")]
public partial class ShowTrainingLogPage : ContentPage
{
    private readonly ShowTrainingLogViewModel _viewModel;

    private int _logId; // Declare the private field

    public int LogId
    {
        get => _logId;
        set
        {
            if (_logId != value && value > 0)
            {
                _logId = value;
                Console.WriteLine($"LogId set to: {_logId}");

                // Load details only once when LogId is assigned
                Dispatcher.Dispatch(async () =>
                {
                    await _viewModel.LoadLogDetailsAsync(_logId);
                });
            }
        }
    }

    public ShowTrainingLogPage()
    {
        InitializeComponent();
        _viewModel = new ShowTrainingLogViewModel();
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        Console.WriteLine($"LogId received: {LogId}");

        if (_viewModel.Id != LogId && LogId > 0)
        {
            Console.WriteLine("Loading data...");
            await _viewModel.LoadLogDetailsAsync(LogId);
        }
    }

    private async void OnUpdateButtonClicked(object sender, EventArgs e)
    {
        if (BindingContext is ShowTrainingLogViewModel viewModel)
        {
            if (viewModel.Id > 0)
            {
                await Shell.Current.GoToAsync($"///UpdateTrainingLogPage?logId={viewModel.Id}");
            }
            else
            {
                Console.WriteLine("Invalid logId for navigation.");
                if (Application.Current?.MainPage != null) // Null check for Application.Current.MainPage
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid Training Log ID.", "OK");
                }
                else
                {
                    Console.WriteLine("MainPage is null. Cannot display alert.");
                }
            }
        }
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//TrainingLogListPage");
    }
}
