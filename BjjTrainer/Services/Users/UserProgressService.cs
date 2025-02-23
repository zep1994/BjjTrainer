using BjjTrainer.Models.DTO.Users;
using System.Net.Http.Json;

namespace BjjTrainer.Services.Users
{
    public class UserProgressService : ApiService
    {
        public string userId { get; private set; }


        public async Task<UserProgressDto> GetUserProgressAsync()
        {
            try
            {
                // Get the user ID from preferences
                userId = Preferences.Get("UserId", string.Empty);

                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User ID is not set in preferences.");
                }

                // Make a GET request to the API endpoint
                var response = await HttpClient.GetAsync($"userprogress/{userId}/progress");

                // Check if the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response JSON to TrainingSummaryModel
                    var trainingSummary = await response.Content.ReadFromJsonAsync<UserProgressDto>();
                    return trainingSummary ?? throw new Exception("Training summary is null.");
                }
                else
                {
                    // Extract error message if the request fails
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Server error: {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to fetch training summary: {ex.Message}");
            }
        }
    }
}
