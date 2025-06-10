using BjjTrainer.Models.DTO.Users;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels
{
    public class GraphViewModel
    {
        public string? Title { get; set; }
        public ObservableCollection<DailyUserProgressDto>? Data { get; set; }
    }
}
