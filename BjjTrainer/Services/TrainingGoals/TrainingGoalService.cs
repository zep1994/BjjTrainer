using BjjTrainer.Models.DTO.TrainingGoals;
using BjjTrainer.Models.TrainingGoal;
using System.Net.Http.Json;

namespace BjjTrainer.Services.TrainingGoals
{
    public class TrainingGoalService : ApiService
    {
        public string? userId { get; private set; }

        public async Task<bool> CreateTrainingGoalAsync(CreateTrainingGoalDto dto)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("traininggoal/create", dto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API error: {response.StatusCode} - {errorDetails}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create training goal: {ex.Message}");
            }
        }


        public async Task<List<TrainingGoal>> GetTrainingGoalsAsync()
        {
            try
            {
                var userId = Preferences.Get("UserId", string.Empty);
                if (string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("User ID is missing in Preferences.");
                    return [];
                }

                var url = $"TrainingGoal/user/{userId}";
                Console.WriteLine($"Fetching goals from URL: {url}");

                var response = await HttpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var goals = await response.Content.ReadFromJsonAsync<List<TrainingGoal>>();
                if (goals == null || !goals.Any())
                {
                    Console.WriteLine("No goals found.");
                    return [];
                }

                foreach (var goal in goals)
                {
                    Console.WriteLine($"Goal: {goal.Notes}, Moves Count: {goal.UserTrainingGoalMoves?.Count ?? 0}");
                }

                return goals;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching training goals: {ex.Message}");
                return [];
            }
        }

        public async Task<TrainingGoalDto> GetTrainingGoalByIdAsync(int goalId)
        {
            try
            {
                return await HttpClient.GetFromJsonAsync<TrainingGoalDto>($"traininggoal/{goalId}")
                    ?? throw new Exception("Training goal not found.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching training goal: {ex.Message}");
            }
        }

        public async Task<bool> UpdateTrainingGoalAsync(int goalId, CreateTrainingGoalDto dto)
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"traininggoal/{goalId}", dto);

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API error: {response.StatusCode} - {errorDetails}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update training goal: {ex.Message}");
            }
        }

        public async Task<bool> DeleteTrainingGoalAsync(int goalId)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"traininggoal/{goalId}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorDetails = await response.Content.ReadAsStringAsync();
                    throw new Exception($"API error: {response.StatusCode} - {errorDetails}");
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete training goal: {ex.Message}");
            }
        }

    }
}