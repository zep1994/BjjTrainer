using BjjTrainer.ViewModels.TrainingGoals;

namespace BjjTrainer.Views.TrainingGoals;

public partial class UpdateTrainingGoalPage : ContentPage
{
    private readonly UpdateTrainingGoalViewModel _viewModel;

    public UpdateTrainingGoalPage(int goalId)
    {
        InitializeComponent();
        _viewModel = new UpdateTrainingGoalViewModel(goalId);
        BindingContext = _viewModel;
    }

    private async void OnSaveGoalClicked(object sender, EventArgs e)
    {
        var success = await _viewModel.SaveGoalAsync();
        if (success)
        {
            await DisplayAlert("Success", "Training goal updated successfully.", "OK");
            await Navigation.PopAsync(); 
        }
        else
        {
            await DisplayAlert("Error", "Failed to update training goal.", "OK");
        }
    }
}
