﻿using BjjTrainer_API.Models.DTO.Moves;
using BjjTrainer_API.Models.DTO.TrainingLogDTOs;

namespace BjjTrainer_API.Models.DTO.Coaches
{
    public class CoachEventDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public int? SchoolId { get; set; }

        // Attendees with minimal info (Username + Check-in time)
        public List<CheckInDto> CheckIns { get; set; } = [];

        // Moves practiced in this event (Only Name and ID)
        public List<LogMoveDto>? Moves { get; set; } = [];
    }
}
