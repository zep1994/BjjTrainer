using BjjTrainer.Models.DTO.Events;
using BjjTrainer.ViewModels.Events;
using Syncfusion.Maui.Scheduler;

namespace BjjTrainer.Views.Events
{
    public partial class CreateEventPage : ContentPage
    {
        private readonly CreateEventViewModel _viewModel;

        public CreateEventPage(DateTime start, DateTime end)
        {
            InitializeComponent();
            _viewModel = new CreateEventViewModel
            {
                StartDate = start,
                StartTime = start.TimeOfDay,
                EndDate = end,
                EndTime = end.TimeOfDay
            };

            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is CalendarViewModel vm)
            {
                await vm.LoadAppointments();
            }
        }

        private async void OnSaveEventClicked(object sender, EventArgs e)
        {
            // Build the DTO with correct property names and logic
            var dto = new CalendarEventCreateDTO
            {
                Title = _viewModel.Title,
                Description = _viewModel.Description,
                StartDate = _viewModel.StartDate.Date,
                StartTime = _viewModel.StartTime,
                EndDate = _viewModel.EndDate.Date,
                EndTime = _viewModel.EndTime,
                IsAllDay = _viewModel.IsAllDay,
                IncludeTrainingLog = _viewModel.IncludeTrainingLog,
                MoveIds = _viewModel.IncludeTrainingLog
                    ? _viewModel.AvailableMoves.Where(m => m.IsSelected).Select(m => m.Id).ToList()
                    : new List<int>()
            };

            var success = await _viewModel.SaveEventAsync(dto);

            if (success)
            {
                await DisplayAlert("Success", "Event saved successfully.", "OK");

                if (Application.Current.MainPage.Navigation.NavigationStack
                    .FirstOrDefault(x => x is CalendarPage) is CalendarPage calendarPage)
                {
                    if (calendarPage.BindingContext is CalendarViewModel calendarViewModel)
                    {
                        await calendarViewModel.LoadAppointments();
                    }
                }

                await Navigation.PushAsync(new CalendarPage());
            }
            else
            {
                await DisplayAlert("Error", "Failed to save event.", "OK");
            }
        }

        private async void OnCancelEventClicked(object sender, EventArgs e)
        {
            // Navigate back without saving
            await Navigation.PushAsync(new CalendarPage());
        }
    }
}

