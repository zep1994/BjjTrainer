using BjjTrainer.ViewModels.TrainingGoals;
using System.Runtime.Versioning;

namespace BjjTrainer.Views.TrainingGoals;

public partial class TrainingGoalListPage : ContentPage
{
    private readonly TrainingGoalListViewModel _viewModel;

    public TrainingGoalListPage()
    {
        InitializeComponent();

        if (OperatingSystem.IsAndroidVersionAtLeast(21) || OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763, 0))
        {
            _viewModel = new TrainingGoalListViewModel(Navigation);
        }
        else
        {
            _viewModel = new TrainingGoalListViewModel(new NullNavigation());
        }

        BindingContext = _viewModel;

        Task.Run(async () => await _viewModel.LoadGoalsAsync());
    }

    [SupportedOSPlatform("android21.0")]
    [SupportedOSPlatform("windows10.0.17763.0")]
    private async void OnDeleteGoalClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int goalId)
        {
            var confirm = await DisplayAlert("Confirm Delete", "Are you sure you want to delete this goal?", "Yes", "No");
            if (confirm)
            {
                await ((TrainingGoalListViewModel)BindingContext).DeleteGoalAsync(goalId);
            }
        }
    }

    [SupportedOSPlatform("android21.0")]
    [SupportedOSPlatform("windows10.0.17763.0")]
    private async void OnSaveGoalClicked(object sender, EventArgs e)
    {
        if (OperatingSystem.IsAndroidVersionAtLeast(21) || OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763, 0))
        {
            await Navigation.PushAsync(new TrainingGoalPage());
        }
    }

    [SupportedOSPlatform("android21.0")]
    [SupportedOSPlatform("windows10.0.17763.0")]
    private async void OnViewGoalClicked(object sender, EventArgs e)
    {
        if (sender is Button button && button.CommandParameter is int goalId)
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(21) || OperatingSystem.IsWindowsVersionAtLeast(10, 0, 17763, 0))
            {
                await Navigation.PushAsync(new ShowTrainingGoalPage(goalId));
            }
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        Task.Run(async () => await _viewModel.LoadGoalsAsync());
    }
}

public class NullNavigation : INavigation
{
    public Task<Page> PopAsync(bool animated = true) => Task.FromResult<Page>(null!);
    public Task<Page> PopModalAsync(bool animated = true) => Task.FromResult<Page>(null!);
    public Task PopToRootAsync(bool animated = true) => Task.CompletedTask;
    public Task PushAsync(Page page, bool animated = true) => Task.CompletedTask;
    public Task PushModalAsync(Page page, bool animated = true) => Task.CompletedTask;

    public void InsertPageBefore(Page page, Page before)
    {
        throw new NotImplementedException();
    }

    public Task<Page> PopAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Page> PopModalAsync()
    {
        throw new NotImplementedException();
    }

    public Task PopToRootAsync()
    {
        throw new NotImplementedException();
    }

    public Task PushAsync(Page page)
    {
        throw new NotImplementedException();
    }

    public Task PushModalAsync(Page page)
    {
        throw new NotImplementedException();
    }

    public void RemovePage(Page page)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<Page> NavigationStack => new List<Page>();
    public IReadOnlyList<Page> ModalStack => new List<Page>();
}
