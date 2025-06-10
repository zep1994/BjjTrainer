using BjjTrainer.Models.Lessons;
using System.Diagnostics;
using System.Net.Http.Json;

namespace BjjTrainer.Services.Lessons
{
    public class LessonService : ApiService
    {
        public LessonService() : base() { }

        public async Task<List<Lesson>> GetAllLessons()
        {
            if (HttpClient == null)
            {
                // Log error or throw an exception
                Debug.WriteLine("HttpClient is not initialized.");
                return []; // Return empty list or handle as needed
            }

            try
            {
                var lessons = await HttpClient.GetFromJsonAsync<List<Lesson>>("lessons");
                if (lessons != null)
                {
                    return lessons;
                } else
                {
                    return [];
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Debug.WriteLine($"Error fetching lessons: {ex.Message}");
                return [];
            }
        }

        public async Task<List<LessonSection>> GetLessonSectionsAsync(int lessonId)
        {
            try
            {
                var response = await HttpClient.GetAsync($"lessonsections/lesson/{lessonId}");

                if (response.IsSuccessStatusCode)
                {
                    var sections = await response.Content.ReadFromJsonAsync<List<LessonSection>>();
                    return sections ?? [];
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error fetching lesson sections: {errorMessage}");
                    return [];
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception in GetLessonSectionsAsync: {ex.Message}");
                return [];
            }
        }

        public async Task<bool> AddLessonToFavoritesAsync(string userId, int lessonId)
        {
            var url = $"api/users/{userId}/favorites/{lessonId}";

            try
            {
                var response = await HttpClient.PostAsync(url, null);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                // Handle exceptions and potentially log them for debugging
                Console.WriteLine($"Error adding lesson to favorites: {ex.Message}");
                return false;
            }
        }
    }
}
