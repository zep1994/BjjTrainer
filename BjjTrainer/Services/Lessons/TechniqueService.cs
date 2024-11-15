using BjjTrainer.Models.Lessons;
using System.Net.Http.Json;

namespace BjjTrainer.Services.Lessons
{
    public class TechniqueService : ApiService
    {
        public async Task<List<Technique>> GetAllTechniquesAsync()
        {
            return await HttpClient.GetFromJsonAsync<List<Technique>>("technique");
        }

        public async Task<Technique> GetTechniqueByIdAsync(int id)
        {
            return await HttpClient.GetFromJsonAsync<Technique>($"technique/{id}");
        }

        public async Task CreateTechniqueAsync(Technique technique)
        {
            await HttpClient.PostAsJsonAsync("technique", technique);
        }

        public async Task UpdateTechniqueAsync(Technique technique)
        {
            await HttpClient.PutAsJsonAsync($"technique/{technique.Id}", technique);
        }

        public async Task DeleteTechniqueAsync(int id)
        {
            await HttpClient.DeleteAsync($"technique/{id}");
        }
    }
}