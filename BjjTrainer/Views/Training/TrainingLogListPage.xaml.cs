using BjjTrainer.ViewModels.TrainingLogs;

namespace BjjTrainer.Views.Training
{
    public partial class TrainingLogListPage : ContentPage
    {
        public TrainingLogListPage()
        {
            InitializeComponent();
            BindingContext = new TrainingLogListViewModel(Navigation);
        }

        private async void OnAddLogTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TrainingLogFormPage());
        }

        private async void OnViewLogClicked(object sender, EventArgs e)
        {
            if (sender is ImageButton button && button.CommandParameter is int logId)
            {
                Console.WriteLine($"Navigating to ShowTrainingLogPage with logId: {logId}");
                await Shell.Current.GoToAsync($"///ShowTrainingLogPage?logId={logId}");
            }
        }
    }
}
