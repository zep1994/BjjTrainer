namespace BjjTrainer.Controls
{
    public partial class FlyoutHeader : ContentView
    {
        public FlyoutHeader()
        {
            InitializeComponent();
        }

        // Navigate to the MainPage
        private async void OnHomeButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        // Navigate to the LoginPage after clearing the AuthToken
        private async void OnLogoutButtonClicked(object sender, EventArgs e)
        {
            Preferences.Remove("AuthToken"); // Clear the token for logout
            await Shell.Current.GoToAsync("///LoginPage");
        }

        private async void OnFavoriteLessonsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//FavoritesPage");
        }

        private async void OnTrainingLogClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//TrainingLogFormPage");
        }

        private async void OnProgressDashClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserProgressPage");
        }        
        
        private async void OnMovesClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MovesPage");
        }

        private async void OnGoalsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//TrainingGoalListPage");
        }
        
        private async void OnCalendarClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//CalendarPage");
        }

        // Navigate to UserProfilePage when the profile image is tapped
        private async void OnProfileImageTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserProfilePage");
        }
    }
}
