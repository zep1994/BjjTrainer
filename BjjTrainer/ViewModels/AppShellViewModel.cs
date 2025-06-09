using MvvmHelpers;

namespace BjjTrainer.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private string _profileImage = string.Empty; 
        public string ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }

        private string _title = string.Empty; 
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isProfileVisible;
        public bool IsProfileVisible
        {
            get => _isProfileVisible;
            set => SetProperty(ref _isProfileVisible, value);
        }

        public AppShellViewModel()
        {
            ProfileImage = "default_profile.png";
            Title = "Roll Call";
        }
    }
}
