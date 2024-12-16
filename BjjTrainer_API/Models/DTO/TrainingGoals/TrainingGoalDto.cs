﻿namespace BjjTrainer_API.Models.DTO.TrainingGoals
{
    public class TrainingGoalDto
    {
        public int Id { get; set; }
        public DateTime GoalDate { get; set; }
        public string Notes { get; set; } = string.Empty;
        public List<int> MoveIds { get; set; } = new List<int>();
        public List<MoveDto> Moves { get; set; } = new List<MoveDto>();
    }

}
