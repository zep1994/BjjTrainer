using BjjTrainer.Models.Schools;
using BjjTrainer.Models.Users;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BjjTrainer.Services.Schools
{
    public class SchoolService : ApiService
    {
        // Get all schools
        public async Task<List<School>> GetAllSchoolsAsync()
        {
            try
            {
                var schools = await HttpClient.GetFromJsonAsync<List<School>>("school");
                return schools ?? [];
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching schools: {ex.Message}");
            }
        }

        // **New Method**: Fetch school by CoachId (String)
        public async Task<School> GetSchoolByCoachIdAsync(string coachId)
        {
            try
            {
                if (string.IsNullOrEmpty(coachId))
                {
                    Console.WriteLine("Error: CoachId is invalid.");
                    throw new Exception("Invalid coach ID.");
                }

                var school = await HttpClient.GetFromJsonAsync<School>($"school/coach/{coachId}");
                if (school == null)
                {
                    Console.WriteLine($"Error: No school found for CoachId {coachId}");
                    throw new Exception("School not found.");
                }

                return school;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching school by CoachId: {ex.Message}");
                throw;
            }
        }

        // Create a new school
        public async Task CreateSchoolAsync(School school)
        {
            try
            {
                var response = await HttpClient.PostAsJsonAsync("school/create", school);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating school: {error}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error creating school: {ex.Message}");
            }
        }

        // Update an existing school
        public async Task UpdateSchoolAsync(School school)
        {
            try
            {
                if (school == null)
                {
                    Console.WriteLine("Error: School object is null.");
                    throw new Exception("Invalid school data.");
                }

                // Attach Authorization Token
                var token = Preferences.Get("AuthToken", string.Empty);
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await HttpClient.PutAsJsonAsync("school/update", school);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error updating school: {error}");
                    throw new Exception($"Error updating school: {error}");
                }

                Console.WriteLine("School updated successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating school: {ex.Message}");
                throw;
            }
        }

        // Delete a school
        public async Task DeleteSchoolAsync(int id)
        {
            try
            {
                var response = await HttpClient.DeleteAsync($"school/delete/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error deleting school: {error}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting school: {ex.Message}");
            }
        }

        public async Task<List<User>> GetStudentsAsync()
        {
            try
            {
                // Attach Authorization Token
                var token = Preferences.Get("AuthToken", string.Empty);
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var students = await HttpClient.GetFromJsonAsync<List<User>>("school/students");
                if (students == null || students.Count == 0)
                {
                    Console.WriteLine("No students found.");
                    return [];
                }

                return students;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching students: {ex.Message}");
                return [];
            }
        }
    }
}
