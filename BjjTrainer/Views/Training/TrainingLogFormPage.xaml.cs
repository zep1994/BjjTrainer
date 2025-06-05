using BjjTrainer.Services.Trainings;
using BjjTrainer.ViewModels.TrainingLogs;

namespace BjjTrainer.Views.Training
{
    public partial class TrainingLogFormPage : ContentPage
    {
        private readonly TrainingLogFormViewModel _viewModel;
        private readonly TrainingService _trainingService;

        public TrainingLogFormPage()
        {
            InitializeComponent();
            _viewModel = new TrainingLogFormViewModel();
            BindingContext = _viewModel;

            _trainingService = new TrainingService();
        }

        private async void OnSubmitButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Retrieve the ApplicationUserId from Preferences
                string applicationUserId = Preferences.Get("UserId", string.Empty);

                if (string.IsNullOrEmpty(applicationUserId))
                {
                    // Handle the case where ApplicationUserId is not found or is empty
                    // You may want to show an error message or redirect to login
                    await DisplayAlert("Error", "User not authenticated. Please log in.", "OK");
                    return;
                }

                var selectedMoves = _viewModel.Moves.Where(m => m.IsSelected).Select(m => m.Id).ToList();

                // Convert DateOnly to string in the format required by the API
                string dateString = _viewModel.Date.ToString("yyyy-MM-dd");

                var trainingLog = new
                {
                    ApplicationUserId = applicationUserId,
                    Date = dateString,  // The Date is now a string in "yyyy-MM-dd" format
                    TrainingTime = _viewModel.TrainingTime ?? 0,  // Defaults to 0 if null
                    RoundsRolled = _viewModel.RoundsRolled ?? 0,
                    Submissions = _viewModel.Submissions ?? 0,
                    Taps = _viewModel.Taps ?? 0,
                    Notes = _viewModel.Notes ?? string.Empty,
                    MoveIds = selectedMoves
                };

                // Submit via TrainingService
                var success = await _trainingService.SubmitTrainingLogAsync(trainingLog);

                if (success)
                {
                    await DisplayAlert("Success", "Training log submitted successfully!", "OK");
                    await Navigation.PushAsync(new MainPage());
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
