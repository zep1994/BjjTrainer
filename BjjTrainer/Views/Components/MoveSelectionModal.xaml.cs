using BjjTrainer.Models.Moves.BjjTrainer.Models.DTO.Moves;
using BjjTrainer.ViewModels.Components;
using BjjTrainer.Services.Trainings;
using System.Collections.ObjectModel;
using System.Linq;

namespace BjjTrainer.Views.Components
{
    public partial class MoveSelectionModal : ContentPage
    {
        public MoveSelectionViewModel ViewModel { get; private set; }
        private readonly TrainingService _trainingService;
        private ObservableCollection<int> _selectedMoveIds;
        private ObservableCollection<int> _toggledMoveIds = new();
        private readonly int _logId;

        public MoveSelectionModal(ObservableCollection<int> selectedMoveIds, int logId)
        {
            InitializeComponent();
            _trainingService = new TrainingService();
            _selectedMoveIds = selectedMoveIds;
            _logId = logId;

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
                    move.IsSelected = _selectedMoveIds.Contains(move.Id);
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

                // Update the list to reflect changes immediately
                ViewModel.RefreshList();

                Console.WriteLine($"Move Toggled: {move.Name} - IsSelected: {move.IsSelected}");
            }
        }

        // Confirm selection and pass updated move list back
        private async void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            var selectedMoveIds = ViewModel.Moves
                .Where(m => m.IsSelected)
                .Select(m => m.Id)
                .ToList();
            Console.WriteLine($"Selected Moves: {selectedMoveIds}");
            MessagingCenter.Send(this, "MovesUpdated", selectedMoveIds);

            // Navigate back to UpdateTrainingLogPage
            await Shell.Current.GoToAsync($"///UpdateTrainingLogPage?logId={_logId}");
        }
    }
}
