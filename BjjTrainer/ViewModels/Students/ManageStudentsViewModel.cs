using BjjTrainer.Models.Users;
using BjjTrainer.Services.Schools;
using MvvmHelpers;
using System.Collections.ObjectModel;

namespace BjjTrainer.ViewModels.Students
{
    public class ManageStudentsViewModel : BaseViewModel
    {
        private readonly SchoolService _schoolService;
        public ObservableCollection<User> Students { get; set; } = [];

        public ManageStudentsViewModel(SchoolService schoolService)
        {
            _schoolService = schoolService;
            Task.Run(async () => await LoadStudents());
        }

        private async Task LoadStudents()
        {
            try
            {
                var students = await _schoolService.GetStudentsAsync();
                Students.Clear();
                foreach (var student in students)
                {
                    Students.Add(student);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading students: {ex.Message}");
            }
        }
    }
}
