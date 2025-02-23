using BjjTrainer.Models.DTO.Moves;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.Components
{
    public partial class MoveSelectionViewModel : BaseViewModel
    {
        public ObservableCollection<UpdateMoveDto> Moves { get; set; }

        public MoveSelectionViewModel()
        {
            Moves = [];
        }

        public MoveSelectionViewModel(ObservableCollection<UpdateMoveDto> selectedMoves)
        {
            Moves = selectedMoves;
        } 

        public void RefreshList()
        {
            OnPropertyChanged(nameof(Moves));
        }
    }
}
