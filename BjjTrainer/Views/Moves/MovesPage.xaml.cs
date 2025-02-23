using BjjTrainer.ViewModels.Moves;

namespace BjjTrainer.Views.Moves;

public partial class MovesPage : ContentPage
{
    private readonly MovesViewModel _viewModel;

    public MovesPage() : this(new MovesViewModel()) { }

    public MovesPage(MovesViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadMovesAsync(); 
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        if (BindingContext is MovesViewModel viewModel)
        {
            viewModel.SearchText = e.NewTextValue;
        }
    }
}
