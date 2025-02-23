using System.Collections.ObjectModel;
using System.Threading.Tasks;
using BjjTrainer.Models.Schools;
using BjjTrainer.Services.Schools;
using MvvmHelpers;
using System.Windows.Input;
using BjjTrainer.Views.Schools;

namespace BjjTrainer.ViewModels.Schools
{
    public partial class ManageSchoolsViewModel : BaseViewModel
    {
        private readonly SchoolService _schoolService;

        public ObservableCollection<School> Schools { get; set; } = [];

        public ICommand CreateSchoolCommand { get; }
        public ICommand EditSchoolCommand { get; }
        public ICommand DeleteSchoolCommand { get; }

        public ManageSchoolsViewModel(SchoolService schoolService)
        {
            _schoolService = schoolService;

            CreateSchoolCommand = new Command(async () => await CreateSchool());
            EditSchoolCommand = new Command<School>(async (school) => await EditSchool(school));

            LoadSchools();
        }

        private async void LoadSchools()
        {
            try
            {
                var schools = await _schoolService.GetAllSchoolsAsync();
                Schools.Clear();
                foreach (var school in schools)
                {
                    Schools.Add(school);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading schools: {ex.Message}");
            }
        }

        private async Task CreateSchool()
        {
            await Shell.Current.GoToAsync(nameof(CreateSchoolPage));
        }

        private async Task EditSchool(School school)
        {
            await Shell.Current.GoToAsync($"{nameof(UpdateSchoolPage)}?id={school.Id}");
        }

        public async Task DeleteSchoolAsync(School school)
        {
            try
            {
                await _schoolService.DeleteSchoolAsync(school.Id);
                Schools.Remove(school);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting school: {ex.Message}");
            }
        }
    }
}
