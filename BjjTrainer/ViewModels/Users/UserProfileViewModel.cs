using BjjTrainer.Services.Users;
using MvvmHelpers;

namespace BjjTrainer.ViewModels.Users
{
    public partial class UserProfileViewModel : BaseViewModel
    {
        private readonly UserService _userService;

        private string username = string.Empty; 
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        private string email = string.Empty; 
        public string Email
        {
            get => email;
            set => SetProperty(ref email, value);
        }

        private string belt = "White";
        public string Belt
        {
            get => belt;
            set => SetProperty(ref belt, value);
        }

        private int beltStripes;
        public int BeltStripes
        {
            get => beltStripes;
            set => SetProperty(ref beltStripes, value);
        }

        private int trainingHoursThisWeek;
        public int TrainingHoursThisWeek
        {
            get => trainingHoursThisWeek;
            set => SetProperty(ref trainingHoursThisWeek, value);
        }

        private string preferredTrainingStyle = string.Empty; 
        public string PreferredTrainingStyle
        {
            get => preferredTrainingStyle;
            set => SetProperty(ref preferredTrainingStyle, value);
        }

        private string profilePictureUrl = string.Empty; 
        public string ProfilePictureUrl
        {
            get => profilePictureUrl;
            set => SetProperty(ref profilePictureUrl, value);
        }

        private DateTime? lastLoginDate;
        public DateTime? LastLoginDate
        {
            get => lastLoginDate;
            set => SetProperty(ref lastLoginDate, value);
        }

        private int totalSubmissions;
        public int TotalSubmissions
        {
            get => totalSubmissions;
            set => SetProperty(ref totalSubmissions, value);
        }

        private int totalTaps;
        public int TotalTaps
        {
            get => totalTaps;
            set => SetProperty(ref totalTaps, value);
        }

        private double totalTrainingTime;
        public double TotalTrainingTime
        {
            get => totalTrainingTime;
            set => SetProperty(ref totalTrainingTime, value);
        }

        private int totalRoundsRolled;
        public int TotalRoundsRolled
        {
            get => totalRoundsRolled;
            set => SetProperty(ref totalRoundsRolled, value);
        }

        public UserProfileViewModel()
        {
            _userService = new UserService();
            _ = LoadUserDataAsync();
        }

        private async Task LoadUserDataAsync()
        {
            var token = Preferences.Get("AuthToken", string.Empty);
            if (!string.IsNullOrEmpty(token))
            {
                var userId = UserService.GetUserIdFromToken(token);
                var user = await _userService.GetUserByIdAsync(userId);

                if (user != null)
                {
                    Username = user.UserName ?? string.Empty;
                    Email = user.Email ?? string.Empty;
                    Belt = user.Belt ?? "White";
                    BeltStripes = user.BeltStripes;
                    TrainingHoursThisWeek = user.TrainingHoursThisWeek;
                    PreferredTrainingStyle = user.PreferredTrainingStyle ?? string.Empty;
                    ProfilePictureUrl = string.IsNullOrEmpty(user.ProfilePictureUrl)
                        ? "Resources/Images/default_profile.png"
                        : user.ProfilePictureUrl;
                    LastLoginDate = user.LastLoginDate;
                    TotalSubmissions = user.TotalSubmissions;
                    TotalTaps = user.TotalTaps;
                    TotalTrainingTime = user.TotalTrainingTime;
                    TotalRoundsRolled = user.TotalRoundsRolled;

                    OnPropertyChanged(nameof(Username));
                    OnPropertyChanged(nameof(Email));
                    OnPropertyChanged(nameof(Belt));
                    OnPropertyChanged(nameof(BeltStripes));
                    OnPropertyChanged(nameof(TrainingHoursThisWeek));
                    OnPropertyChanged(nameof(PreferredTrainingStyle));
                    OnPropertyChanged(nameof(ProfilePictureUrl));
                    OnPropertyChanged(nameof(LastLoginDate));
                    OnPropertyChanged(nameof(TotalSubmissions));
                    OnPropertyChanged(nameof(TotalTaps));
                    OnPropertyChanged(nameof(TotalTrainingTime));
                    OnPropertyChanged(nameof(TotalRoundsRolled));
                }
            }
        }
    }
}