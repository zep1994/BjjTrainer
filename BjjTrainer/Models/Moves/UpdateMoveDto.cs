using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BjjTrainer.Models.Moves.BjjTrainer.Models.DTO.Moves
{
    public class UpdateMoveDto : INotifyPropertyChanged
    {
        private bool _isSelected;

        public int Id { get; set; }
        public string? Name { get; set; }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
