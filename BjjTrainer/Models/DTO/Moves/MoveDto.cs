namespace BjjTrainer.Models.DTO.Moves
{
    public class MoveDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? SkillLevel { get; set; }
        public List<string>? Tags { get; set; } = [];
        public int TrainingLogCount { get; set; }
    }
}