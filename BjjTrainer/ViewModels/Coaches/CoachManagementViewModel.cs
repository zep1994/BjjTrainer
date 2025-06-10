using BjjTrainer.Models.Schools;
using BjjTrainer.Services.Schools;
using MvvmHelpers;

public class CoachManagementViewModel : BaseViewModel
{
    private readonly SchoolService _schoolService;
    private School? _coachSchool;

    public School? CoachSchool
    {
        get => _coachSchool;
        set
        {
            _coachSchool = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(HasSchool));
        }
    }

    public bool HasSchool => CoachSchool != null;

    // Inject SchoolService via constructor
    public CoachManagementViewModel(SchoolService schoolService)
    {
        _schoolService = schoolService;
        Task.Run(async () => await LoadCoachSchool());
    }

    private async Task LoadCoachSchool()
    {
        try
        {
            var userId = Preferences.Get("UserId", string.Empty);
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("Error: UserId is missing.");
                return;
            }

            Console.WriteLine($"Fetching school for UserId: {userId}");
            var schoolData = await _schoolService.GetSchoolByCoachIdAsync(userId);

            if (schoolData != null)
            {
                Console.WriteLine($"School Loaded: {schoolData.Name}");
                CoachSchool = schoolData;
            }
            else
            {
                Console.WriteLine("Error: No school assigned to this coach.");
                CoachSchool = new School(); // Prevents null issues in UI binding
            }

            OnPropertyChanged(nameof(CoachSchool));
            OnPropertyChanged(nameof(HasSchool));
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading coach's school: {ex.Message}");
        }
    }
}
