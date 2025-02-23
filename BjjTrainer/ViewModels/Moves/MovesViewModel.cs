using BjjTrainer.Models.Moves;
using BjjTrainer.Services.Moves;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.Moves
{
    public partial class MovesViewModel : ObservableObject
    {
        private readonly MoveService _moveService;
        private List<Move> _allMoves = new();

        public ObservableCollection<Move> Moves { get; } = new();
        public ObservableCollection<string> MoveNames { get; } = new();
        public ObservableCollection<Move> FilteredMoves { get; } = new(); // ✅ Fix: Ensure it stays updated

        [ObservableProperty]
        private string searchText = string.Empty;

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        // ✅ **Add Default Constructor**
        public MovesViewModel() : this(new MoveService()) { }

        // ✅ **Existing Constructor**
        public MovesViewModel(MoveService moveService)
        {
            _moveService = moveService;
        }

        [RelayCommand]
        public async Task LoadMovesAsync()
        {
            if (IsBusy) return;

            IsBusy = true;
            try
            {
                Console.WriteLine("Loading moves...");

                // Fetching data from API
                _allMoves = await _moveService.GetAllMovesAsync();

                // Updating UI on the main thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Moves.Clear();
                    MoveNames.Clear();
                    FilteredMoves.Clear(); // ✅ Fix: Ensure it updates

                    foreach (var move in _allMoves)
                    {
                        Moves.Add(move);
                        MoveNames.Add(move.Name);
                        FilteredMoves.Add(move); // ✅ Fix: Populate FilteredMoves immediately
                    }
                });

                Console.WriteLine("Moves loaded successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading moves: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                FilteredMoves.Clear();

                if (string.IsNullOrWhiteSpace(value))
                {
                    foreach (var move in Moves)
                    {
                        FilteredMoves.Add(move);
                    }
                }
                else
                {
                    foreach (var move in Moves.Where(m =>
                        m.Name.Contains(value, StringComparison.OrdinalIgnoreCase) ||
                        m.Description.Contains(value, StringComparison.OrdinalIgnoreCase)))
                    {
                        FilteredMoves.Add(move);
                    }
                }
            });
        }
    }
}
