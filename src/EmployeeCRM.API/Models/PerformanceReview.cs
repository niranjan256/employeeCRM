namespace EmployeeCRM.API.Models
{
    public class PerformanceReview
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int ReviewerId { get; set; }
        public Employee Reviewer { get; set; } = null!;
        public int Rating { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string ReviewPeriod { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;
    }
}
