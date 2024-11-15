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

        public ObservableCollection<Models.Lessons.Lesson> FavoriteLessons { get; set; }
        public ICommand LoadFavoritesCommand { get; }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public FavoritesViewModel()
        {
            _userService = new UserService();
            FavoriteLessons = new ObservableCollection<Models.Lessons.Lesson>();
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
                // Handle any exceptions, such as displaying an alert
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
