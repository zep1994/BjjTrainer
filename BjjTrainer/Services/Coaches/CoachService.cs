﻿using BjjTrainer.Models.DTO.Coaches;
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
                return await response.Content.ReadFromJsonAsync<List<CoachEventDto>>();
            }

            throw new Exception($"Failed to retrieve past events: {await response.Content.ReadAsStringAsync()}");
        }

        public async Task<CoachEventDto> GetEventDetailsAsync(int eventId)
        {
            AttachAuthorizationHeader();
            var response = await HttpClient.GetAsync($"coach/events/details/{eventId}");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CoachEventDto>();
            }

            throw new Exception($"Failed to retrieve event details: {await response.Content.ReadAsStringAsync()}");
        }
    }
}
