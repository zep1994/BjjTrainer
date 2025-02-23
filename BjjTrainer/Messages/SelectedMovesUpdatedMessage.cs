using System.Collections.ObjectModel;
using BjjTrainer.Models.DTO.Moves;

namespace BjjTrainer.Messages
{
    public class SelectedMovesUpdatedMessage
    {
        public ObservableCollection<UpdateMoveDto> Moves { get; }

        public SelectedMovesUpdatedMessage(ObservableCollection<UpdateMoveDto> moves)
        {
            Moves = moves;
        }
    }
}