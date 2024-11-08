﻿using System.ComponentModel.DataAnnotations.Schema;

namespace BjjTrainer_API.Models.Lessons
{
    public class SubLesson
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; } = TimeSpan.FromSeconds(1);
        public string VideoUrl { get; set; } = string.Empty;
        public string DocumentUrl { get; set; } = string.Empty;
        public string[] Tags { get; set; } = [];
        public string SkillLevel { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;

        [ForeignKey("LessonSection")]
        public int LessonSectionId { get; set; }

        // Navigation property for the related LessonSection
        public LessonSection? LessonSection { get; set; }


    }
}
