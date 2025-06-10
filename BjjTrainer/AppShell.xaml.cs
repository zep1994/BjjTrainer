using BjjTrainer.ViewModels;

namespace BjjTrainer
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel();

            Navigated += OnNavigated;
        }

        private void OnNavigated(object? sender, ShellNavigatedEventArgs e)
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            var currentRoute = Shell.Current?.CurrentItem?.Route;

            if (currentRoute == "IMPL_LoginPage" || currentRoute == "IMPL_SignupPage")
            {
                // Enable FlyoutHeader for logged-in pages
                Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
                ProfileImage.IsVisible = false;
            }
            else
            {
                // Enable FlyoutHeader for logged-in pages
                Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
                ProfileImage.IsVisible = true;
            }
        }

        private async void OnProfileIconTapped(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//UserProfilePage");
        }
    }
}
