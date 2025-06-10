namespace BjjTrainer.Models.Users
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public string? UserId { get; set; }  // Assuming the API provides this
    }
}
