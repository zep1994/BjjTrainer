using BjjTrainer.Models.DTO.Coaches;
using System.Net.Http.Json;

namespace BjjTrainer.Services.Coaches
{
    public class CoachService : ApiService
    {
        public async Task<List<CoachEventDto>> GetPastEventsWithDetailsAsync(int schoolId)
        {
            AttachAuthorizationHeader();
            var response = await HttpClient.GetAsync($"coach/events/full/{schoolId}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<CoachEventDto>>();
                return result ?? []; // Ensure a non-null return value
            }

            throw new Exception($"Failed to retrieve past events: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task<CoachEventDto> GetEventDetailsAsync(int eventId)
        {
            AttachAuthorizationHeader();
            var response = await HttpClient.GetAsync($"coach/events/details/{eventId}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CoachEventDto>();
                if (result == null)
                {
                    throw new Exception("Event details response was null.");
                }
                return result;
            }

            throw new Exception($"Failed to retrieve event details: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
