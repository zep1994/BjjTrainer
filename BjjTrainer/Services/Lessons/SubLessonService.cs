using BjjTrainer.Models.DTO;
using BjjTrainer.Models.Lessons;
using System.Net.Http.Json;
using System.Text.Json;

namespace BjjTrainer.Services.Lessons
{
    public class SubLessonService : ApiService
    {
        public async Task<List<SubLesson>> GetSubLessonsBySectionAsync(int lessonSectionId)
        {
            var response = await HttpClient.GetAsync($"sublessons/sections/{lessonSectionId}");
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var subLessons = await response.Content.ReadFromJsonAsync<List<SubLesson>>(options);

            // Ensure the return value is not null
            return subLessons ?? [];
        }

        public async Task<SubLessonDetailsDto> GetSubLessonDetailsByIdAsync(int subLessonId)
        {
            var response = await HttpClient.GetAsync($"sublessons/{subLessonId}/details");
            response.EnsureSuccessStatusCode();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var subLessonDetails = await response.Content.ReadFromJsonAsync<SubLessonDetailsDto>(options);

            // Ensure the return value is not null
            return subLessonDetails ?? throw new InvalidOperationException("SubLessonDetailsDto cannot be null.");
        }
    }
}
