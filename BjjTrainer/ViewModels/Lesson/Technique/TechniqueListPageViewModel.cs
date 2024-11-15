using BjjTrainer.Services.Lessons;
using MvvmHelpers;

namespace BjjTrainer.ViewModels.Lesson.Technique
{
    public partial class TechniqueListPageViewModel : BaseViewModel
    {
        private readonly TechniqueService _techniqueService;

        public ObservableRangeCollection<Models.Lessons.Technique> Techniques { get; private set; }
        public Command LoadTechniquesCommand { get; }

        public TechniqueListPageViewModel(TechniqueService techniqueService)
        {
            _techniqueService = techniqueService;
            Techniques = new ObservableRangeCollection<Models.Lessons.Technique>();
            LoadTechniquesCommand = new Command(async () => await LoadTechniques());
        }

        private async Task LoadTechniques()
        {
            var techniques = await _techniqueService.GetAllTechniquesAsync();
            Techniques.ReplaceRange(techniques);
        }
    }
}
