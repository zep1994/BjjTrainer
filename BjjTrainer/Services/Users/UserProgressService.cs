using BjjTrainer.Models.DTO.Users;
using System.Net.Http.Json;

namespace BjjTrainer.Services.Users
{
    public class UserProgressService : ApiService
    {
        public string? userId { get; private set; }

        public async Task<UserProgressDto> GetUserProgressAsync()
        {
            userId = Preferences.Get("UserId", string.Empty);
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User ID is not set in preferences.");

            var response = await HttpClient.GetAsync($"userprogress/{userId}/progress");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<UserProgressDto>() ?? throw new Exception("Training summary is null.");
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server error: {errorMessage}");
        }

        // Helper for all daily progress endpoints
        private async Task<List<DailyUserProgressDto>> GetDailyProgressAsync(string endpoint)
        {
            userId = Preferences.Get("UserId", string.Empty);
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User ID is not set in preferences.");

            var response = await HttpClient.GetAsync($"userprogress/{userId}/{endpoint}");
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<DailyUserProgressDto>>() ?? new();
            var errorMessage = await response.Content.ReadAsStringAsync();
            throw new Exception($"Server error: {errorMessage}");
        }

        // Training Time
        public Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeLast7DaysAsync() =>
            GetDailyProgressAsync("training-time/daily/last7days");
        public Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeLastMonthAsync() =>
            GetDailyProgressAsync("training-time/daily/lastmonth");
        public Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeLast6MonthsAsync() =>
            GetDailyProgressAsync("training-time/daily/last6months");
        public Task<List<DailyUserProgressDto>> GetDailyTotalTrainingTimeLastYearAsync() =>
            GetDailyProgressAsync("training-time/daily/lastyear");

        // Rounds Rolled
        public Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledLast7DaysAsync() =>
            GetDailyProgressAsync("rounds-rolled/daily/last7days");
        public Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledLastMonthAsync() =>
            GetDailyProgressAsync("rounds-rolled/daily/lastmonth");
        public Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledLast6MonthsAsync() =>
            GetDailyProgressAsync("rounds-rolled/daily/last6months");
        public Task<List<DailyUserProgressDto>> GetDailyTotalRoundsRolledLastYearAsync() =>
            GetDailyProgressAsync("rounds-rolled/daily/lastyear");

        // Submissions
        public Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsLast7DaysAsync() =>
            GetDailyProgressAsync("submissions/daily/last7days");
        public Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsLastMonthAsync() =>
            GetDailyProgressAsync("submissions/daily/lastmonth");
        public Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsLast6MonthsAsync() =>
            GetDailyProgressAsync("submissions/daily/last6months");
        public Task<List<DailyUserProgressDto>> GetDailyTotalSubmissionsLastYearAsync() =>
            GetDailyProgressAsync("submissions/daily/lastyear");

        // Taps
        public Task<List<DailyUserProgressDto>> GetDailyTotalTapsLast7DaysAsync() =>
            GetDailyProgressAsync("taps/daily/last7days");
        public Task<List<DailyUserProgressDto>> GetDailyTotalTapsLastMonthAsync() =>
            GetDailyProgressAsync("taps/daily/lastmonth");
        public Task<List<DailyUserProgressDto>> GetDailyTotalTapsLast6MonthsAsync() =>
            GetDailyProgressAsync("taps/daily/last6months");
        public Task<List<DailyUserProgressDto>> GetDailyTotalTapsLastYearAsync() =>
            GetDailyProgressAsync("taps/daily/lastyear");
    }
}
