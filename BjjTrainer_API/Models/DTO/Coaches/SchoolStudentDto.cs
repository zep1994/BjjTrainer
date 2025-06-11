namespace BjjTrainer_API.Models.DTO.Students
{
    public class SchoolStudentDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public string Belt { get; set; } = string.Empty;
        public int BeltStripes { get; set; }
        public DateOnly? TrainingStartDate { get; set; }
        public DateOnly? LastLoginDate { get; set; }
        public int EventsAttended { get; set; }
    }
}
