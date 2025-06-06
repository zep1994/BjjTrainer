using BjjTrainer.Models.DTO.Events;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace BjjTrainer.Services.Events
{
    public class EventService : ApiService
    {
        public EventService() : base() { }

        // CREATE
        public async Task<bool> CreateEventAsync(CalendarEventCreateDTO newEvent)
        {
            try
            {
                AttachAuthorizationHeader();
                var json = JsonSerializer.Serialize(newEvent);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Send POST request to the specified URL
                var response = await HttpClient.PostAsync("calendar/events/create", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Server error: {errorMessage}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create event: {ex.Message}");
            }
        }

        // SHOW
        public async Task<CalendarEventDto> GetEventByIdAsync(int eventId)
        {
            try
            {
                AttachAuthorizationHeader();
                var response = await HttpClient.GetAsync($"calendar/{eventId}");

                if (response.IsSuccessStatusCode)
                {
                    var eventJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<CalendarEventDto>(eventJson);
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Server error: {errorMessage}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching event: {ex.Message}");
                throw;
            }
        }

        // UPDATE
        public async Task<bool> UpdateEventAsync(int eventId, CalendarEventUpdateDto updatedEvent)
        {
            try
            {
                AttachAuthorizationHeader();

                // Log the object as JSON before sending
                var json = JsonSerializer.Serialize(updatedEvent);
                Console.WriteLine($"[UpdateEventAsync] Sending JSON for eventId={eventId}: {json}");

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await HttpClient.PutAsync($"calendar/events/{eventId}", content);

                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }

        // DELETE
        public async Task<bool> DeleteEventAsync(int eventId)
        {
            try
            {
                AttachAuthorizationHeader();
                // Send DELETE request to delete the event
                var response = await HttpClient.DeleteAsync($"calendar/{eventId}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Server error: {errorMessage}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to delete event: {ex.Message}");
            }
        }

        // GET ALL USER EVENTS
        public async Task<List<CalendarEventDto>> GetAllUserEventsAsync(string userId)
        {
            try
            {
                AttachAuthorizationHeader();
                var response = await HttpClient.GetAsync($"calendar/user/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Raw JSON Response: {jsonResponse}");
                    try
                    {
                        return JsonSerializer.Deserialize<List<CalendarEventDto>>(jsonResponse);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Deserialization error: {ex.Message}");
                        return [];
                    }
                }

                return [];  // Return empty list if request fails
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to retrieve events: {ex.Message}");
            }
        }
    }
}