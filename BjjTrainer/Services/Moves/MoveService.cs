using BjjTrainer.Models.Moves;
using System.Net.Http.Json;

namespace BjjTrainer.Services.Moves
{
    public class MoveService : ApiService
    {
        public async Task<List<Move>> GetAllMovesAsync()
        {
            try
            {
                Console.WriteLine("Fetching moves from API...");
                var response = await HttpClient.GetAsync("moves");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.StatusCode}");
                    return new List<Move>();
                }

                var moves = await response.Content.ReadFromJsonAsync<List<Move>>() ?? [];
                Console.WriteLine($"Successfully fetched {moves.Count} moves.");
                return moves;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching moves: {ex.Message}");
                return new List<Move>();
            }
        }
    }
}
