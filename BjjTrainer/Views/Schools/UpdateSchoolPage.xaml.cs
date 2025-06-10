using BjjTrainer.Models.Schools;
using BjjTrainer.ViewModels.Schools;
using Newtonsoft.Json;

namespace BjjTrainer.Views.Schools
{
    [QueryProperty(nameof(SchoolJson), "schoolJson")]
    public partial class UpdateSchoolPage : ContentPage
    {
        private readonly UpdateSchoolViewModel _viewModel;
        private string _schoolJson = string.Empty; 

      public string SchoolJson
        {
            get => _schoolJson;
            set
            {
                _schoolJson = value;
                if (!string.IsNullOrEmpty(_schoolJson))
                {
                    Console.WriteLine($"Received School JSON: {_schoolJson}");
                    var school = JsonConvert.DeserializeObject<School>(_schoolJson);
                    if (school != null) // Ensure school is not null before calling LoadSchool
                    {
                        LoadSchool(school);
                    }
                    else
                    {
                        Console.WriteLine("Error: Failed to deserialize School JSON into a valid School object.");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Failed to deserialize School JSON.");
                }
            }
        }

        public UpdateSchoolPage(UpdateSchoolViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        private void LoadSchool(School school)
        {
            if (school != null)
            {
                _viewModel.SetSchool(school);
            }
            else
            {
                Console.WriteLine("Error: Invalid school data received.");
            }
        }

        private async void OnSaveClicked(object sender, EventArgs e)
        {
            await _viewModel.SaveSchoolAsync();
            await Shell.Current.GoToAsync("///CoachManagementPage");
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///CoachManagementPage");
        }
    }
}
