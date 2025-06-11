namespace BjjTrainer_API.Models.DTO.Coaches
{
    public class SchoolAttendanceSummaryDto
    {
        public int TotalStudents { get; set; }
        public int TotalEvents { get; set; }
        public int TotalCheckIns { get; set; }
        public double AverageAttendancePerEvent { get; set; }
        public double AverageAttendancePerStudent { get; set; }
        public int TotalEventsAttended { get; set; } 
        public List<SchoolAttendanceRecordDto> Records { get; set; } = new();
    }
}