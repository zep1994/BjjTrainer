using BjjTrainer.Models.Moves;
using BjjTrainer.Services.Moves;
using MvvmHelpers;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace BjjTrainer.ViewModels.Moves
{
    public partial class MovesViewModel : BaseViewModel
    {
        private readonly MoveService _moveService;

        public ObservableCollection<Move> Moves { get; set; } = [];

        public MovesViewModel()
        {
            _moveService = new MoveService();
            _ = LoadMovesAsync(); // Explicitly discard the task to avoid CS4014 warning
        }

        public async Task LoadMovesAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                var moves = await _moveService.GetAllMovesAsync();
                Moves.Clear();
                foreach (var move in moves)
                {
                    Moves.Add(move);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading moves: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}