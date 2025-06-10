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
            var currentRoute = Current?.CurrentItem?.Route;

            if (currentRoute == "IMPL_LoginPage" || currentRoute == "IMPL_SignupPage")
            {
                Shell.SetFlyoutBehavior(this, FlyoutBehavior.Disabled);
                ProfileImage.IsVisible = false;
            }
            else
            {
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
