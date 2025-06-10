using BjjTrainer.Models.Lessons;
using BjjTrainer.Services.Users;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BjjTrainer.ViewModels
{
    public class FavoritesViewModel : BaseViewModel
    {
        private readonly UserService _userService;
        private bool _isRefreshing;

        public ObservableCollection<Lesson> FavoriteLessons { get; set; }
        public ICommand LoadFavoritesCommand { get; }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public FavoritesViewModel()
        {
            _userService = new UserService();
            FavoriteLessons = [];
            LoadFavoritesCommand = new Command(async () => await LoadFavoritesAsync());
        }

        public async Task LoadFavoritesAsync()
        {
            if (IsRefreshing) return;  // Prevent multiple refreshes
            IsRefreshing = true;

            try
            {
                FavoriteLessons.Clear();

                var token = Preferences.Get("AuthToken", string.Empty);
                var userId = UserService.GetUserIdFromToken(token);

                if (!string.IsNullOrEmpty(userId))
                {
                    var favorites = await _userService.GetUserFavoritesAsync(userId);
                    foreach (var lesson in favorites)
                    {
                        FavoriteLessons.Add(lesson);
                    }
                }
            }
            catch (Exception ex)
            {
                // Ensure Application.Current and MainPage are not null before accessing them
                if (Application.Current?.MainPage != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
                else
                {
                    // Log or handle the case where Application.Current or MainPage is null
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
