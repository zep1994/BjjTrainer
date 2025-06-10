using BjjTrainer.Models.DTO.Coaches;
using BjjTrainer.Services.Coaches;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.Coaches
{
    public partial class CoachViewModel(CoachService coachService) : ObservableObject
    {
        private readonly CoachService _coachService = coachService;
        private ObservableCollection<CoachEventDto>? _pastEvents;
        public ObservableCollection<CoachEventDto>? PastEvents
        {
            get => _pastEvents;
            set => SetProperty(ref _pastEvents, value);
        }

        [RelayCommand]
        public async Task LoadPastEvents(int schoolId)
        {
            try
            {
                var events = await _coachService.GetPastEventsWithDetailsAsync(schoolId);
                PastEvents = new ObservableCollection<CoachEventDto>(events);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading past events: {ex.Message}");
            }
        }
    }
}
