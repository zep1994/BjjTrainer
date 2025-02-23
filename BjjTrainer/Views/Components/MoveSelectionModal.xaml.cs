using BjjTrainer.Messages;
using BjjTrainer.Models.DTO.Moves;
using BjjTrainer.Services.Trainings;
using BjjTrainer.ViewModels.Components;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BjjTrainer.Views.Components
{
    public partial class MoveSelectionModal : ContentPage
    {
        public MoveSelectionViewModel ViewModel { get; private set; }
        private readonly TrainingService _trainingService;
        private ObservableCollection<UpdateMoveDto> _selectedMoves;
        private readonly int _logId;

        public MoveSelectionModal(ObservableCollection<UpdateMoveDto>? selectedMoves = null, int logId = 0)
        {
            InitializeComponent();
            _trainingService = new TrainingService();
            _selectedMoves = selectedMoves ?? [];
            _logId = logId;

            ViewModel = new MoveSelectionViewModel(_selectedMoves);
            BindingContext = ViewModel;

            LoadAllMovesAsync();
        }

        private async void LoadAllMovesAsync()
        {
            try
            {
                var allMoves = await _trainingService.GetAllMovesAsync();
                var moveDtos = new ObservableCollection<UpdateMoveDto>(allMoves);

                foreach (var move in moveDtos)
                {
                    move.IsSelected = _selectedMoves.Any(m => m.Id == move.Id);
                }

                ViewModel = new MoveSelectionViewModel(moveDtos);
                BindingContext = ViewModel;
                MoveListView.ItemsSource = ViewModel.Moves;
                ViewModel.RefreshList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading moves: {ex.Message}");
                await DisplayAlert("Error", $"Failed to load moves: {ex.Message}", "OK");
            }
        }

        // Toggle Move Selection
        private void OnToggleMoveClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.BindingContext is UpdateMoveDto move)
            {
                move.IsSelected = !move.IsSelected;

                var existingMove = _selectedMoves.FirstOrDefault(m => m.Id == move.Id);
                if (move.IsSelected && existingMove == null)
                {
                    _selectedMoves.Add(move);
                }
                else if (!move.IsSelected && existingMove != null)
                {
                    _selectedMoves.Remove(existingMove);
                }

                ViewModel.RefreshList();
            }
        }

        // Confirm selection and pass updated move list back
        private async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            var selectedMoves = ViewModel.Moves
                .Where(m => m.IsSelected)
                .ToList();

            // Send updated moves back to the ViewModel
            WeakReferenceMessenger.Default.Send(new SelectedMovesUpdatedMessage(
                new ObservableCollection<UpdateMoveDto>(selectedMoves)
            ));

            // Close the modal
            await Navigation.PopModalAsync();
        }
    }
}