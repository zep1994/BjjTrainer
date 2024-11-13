using BjjTrainer.ViewModels;
using BjjTrainer.Views.Lessons;

namespace BjjTrainer.Views.Users
{
    public partial class FavoritesPage : ContentPage
    {
        private readonly FavoritesViewModel _viewModel;

        public FavoritesPage()
        {
            InitializeComponent();
            _viewModel = BindingContext as FavoritesViewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_viewModel != null)
            {
                await _viewModel.LoadFavoritesAsync();
            }
        }

        // Handle the button click event to navigate back to the LessonsPage
        private async void OnBackToLessonsClicked(object sender, EventArgs e)
        {
            // Navigate back to LessonsPage
            //await Navigation.PopAsync(); // If you're using navigation stack
            await Navigation.PushAsync(new LessonsPage()); 
        }
    }
}
