namespace BjjTrainer.Models.Users
{
    public class SignupResponse
    {
        public string? Token { get; set; }
        public string Message { get; set; } = "Signup successful!";
    }
}
