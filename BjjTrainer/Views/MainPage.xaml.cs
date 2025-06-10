using BjjTrainer.ViewModels;
using System.Runtime.Versioning;

namespace BjjTrainer.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly MainPageViewModel _viewModel;

        public MainPage()
        {
            InitializeComponent();
            _viewModel = new MainPageViewModel();
            SetBindingContext();
        }

        [SupportedOSPlatform("windows10.0.17763.0")]
        [SupportedOSPlatform("android21.0")]
        private void SetBindingContext()
        {
            BindingContext = _viewModel;
        }

        [SupportedOSPlatform("windows10.0.17763.0")]
        [SupportedOSPlatform("android21.0")]
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is MainPageViewModel vm)
                await vm.InitializeAsync();
        }
    }
}