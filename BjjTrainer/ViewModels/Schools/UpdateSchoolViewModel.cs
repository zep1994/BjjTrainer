using BjjTrainer.Models.Schools;
using BjjTrainer.Services.Schools;
using MvvmHelpers;

namespace BjjTrainer.ViewModels.Schools
{
    public class UpdateSchoolViewModel : BaseViewModel
    {
        private readonly SchoolService _schoolService;
        private School _school = null!; // Initialize with null-forgiving operator

        public School School
        {
            get => _school;
            set
            {
                _school = value;
                OnPropertyChanged();
            }
        }

        public UpdateSchoolViewModel(SchoolService schoolService)
        {
            _schoolService = schoolService;
            School = new School(); // Ensures object is initialized
        }

        public void SetSchool(School school)
        {
            if (school != null)
            {
                School = school;
                OnPropertyChanged(nameof(School));
                Console.WriteLine($"School Loaded: {School.Name}");
            }
        }

        public async Task SaveSchoolAsync()
        {
            try
            {
                if (School == null)
                {
                    Console.WriteLine("Error: Invalid school data.");
                    return;
                }

                Console.WriteLine($"Updating school: {School.Name}");
                await _schoolService.UpdateSchoolAsync(School);
                await Shell.Current.GoToAsync("//CoachManagementPage");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating school: {ex.Message}");
            }
        }
    }
}
