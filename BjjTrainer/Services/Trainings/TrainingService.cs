using BjjTrainer.Models.DTO.TrainingLog;
using BjjTrainer.Models.Moves.BjjTrainer.Models.DTO.Moves;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BjjTrainer.Services.Trainings
{
    public class TrainingService : ApiService
    {
        // ******************************** GET ALL TRAINING LOGS BY USER ********************************
        public async Task<List<TrainingLogDto>> GetTrainingLogsAsync(string userId)
        {
            try
            {
                var response = await HttpClient.GetFromJsonAsync<List<TrainingLogDto>>($"traininglog/list/{userId}");
                return response ?? [];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching training logs: {ex.Message}");
            }
        }

        // ******************************** GET SINGLE TRAINING LOG BY ID ********************************
        public async Task<TrainingLogDto> GetTrainingLogByIdAsync(int logId)
        {
            try
            {
                var log = await HttpClient.GetFromJsonAsync<TrainingLogDto>($"traininglog/{logId}");

                if (log == null)
                {
                    Console.WriteLine("Training log not found in API response.");
                    throw new Exception("Training log not found.");
                }

                Console.WriteLine($"Training log from API: {log.Date} - {log.TrainingTime} hrs");
                return log;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching training log from API: {ex.Message}");
                throw new Exception($"Error fetching training log: {ex.Message}");
            }
        }

        // ******************************** GET MOVES FOR TRAINING LOG ********************************
        public async Task<UpdateTrainingLogDto> GetTrainingLogMoves(int logId)
        {
            try
            {
                var log = await HttpClient.GetFromJsonAsync<UpdateTrainingLogDto>($"traininglog/log/{logId}");

                if (log == null)
                {
                    Console.WriteLine("Training log not found in API response.");
                    throw new Exception($"Training log with ID {logId} not found.");
                }

                // Log for debugging
                Console.WriteLine($"Fetched training log: Date={log.Date}, MovesCount={log.Moves.Count}");

                return log;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching training log from API: {ex.Message}");
                throw new Exception($"Failed to fetch training log (ID: {logId}): {ex.Message}");
            }
        }

        // ******************************** REMOVE MOVE TO TRAINING LOG ********************************
        public async Task RemoveMoveFromTrainingLog(int logId, int moveId)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"traininglog/{logId}/removemove/{moveId}");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to remove move: {error}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error removing move: {ex.Message}");
            }
        }

        // ******************************** CREATE NEW TRAINING LOG ********************************
        public async Task<bool> SubmitTrainingLogAsync(object trainingLog)
        {
            try
            {
                var json = JsonSerializer.Serialize(trainingLog);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await HttpClient.PostAsync("traininglog/log", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Server error: {errorMessage}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to submit training log: {ex.Message}");
            }
        }

        // ******************************** UPDATE EXISTING TRAINING LOG ********************************
        public async Task UpdateTrainingLogAsync(int logId, UpdateTrainingLogDto updatedLog, bool isCoachLog)
        {
            try
            {
                var response = await HttpClient.PutAsJsonAsync($"traininglog/{logId}", updatedLog);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error updating training log: {error}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating training log: {ex.Message}");
            }
        }

        // ******************************** DELETE TRAINING LOG ********************************
        public async Task DeleteTrainingLogAsync(int logId)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"traininglog/{logId}");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error deleting training log: {error}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting training log: {ex.Message}");
            }
        }

        // ******************************** FETCH ALL MOVES ********************************
        public async Task<List<UpdateMoveDto>> GetAllMovesAsync()
        {
            try
            {
                var moves = await HttpClient.GetFromJsonAsync<List<UpdateMoveDto>>("moves");
                return moves ?? [];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching moves: {ex.Message}");
            }
        }
    }
}
