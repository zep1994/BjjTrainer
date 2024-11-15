using BjjTrainer.Models.Lessons;
using BjjTrainer.Services.Lessons;
using BjjTrainer.Services.Users;
using MvvmHelpers;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BjjTrainer.ViewModels
{
    public partial class LessonsViewModel(LessonService lessonService) : BaseViewModel
    {
        private readonly LessonService _lessonService = lessonService;
        private static readonly UserService userService = new();
        private readonly UserService _userService = userService;


        public ObservableCollection<Models.Lessons.Lesson> Lessons { get; } = [];

        public async Task LoadLessonsAsync()
        {
            var lessons = await _lessonService.GetAllLessons();
            foreach (var lesson in lessons)
            {
                Lessons.Add(lesson);
            }
        }

        public async Task<bool> AddToFavoritesAsync(int lessonId)
        {
            var token = Preferences.Get("AuthToken", string.Empty);
            var userId = Preferences.Get("UserId", string.Empty);

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userId))
            {
                    return await _userService.AddLessonToFavoritesAsync(userId, lessonId);
            }
            return false;
        }
    }
}