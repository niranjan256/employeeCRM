using System.ComponentModel.DataAnnotations;

namespace EmployeeCRM.Shared.DTOs
{
    public class PerformanceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comments { get; set; } = string.Empty;
        public string ReviewPeriod { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
    }

    public class CreatePerformanceDto
    {
        [Required] public int EmployeeId { get; set; }
        [Required] public int ReviewerId { get; set; }
        [Range(1, 5)] public int Rating { get; set; }
        public string Comments { get; set; } = string.Empty;
        [Required] public string ReviewPeriod { get; set; } = string.Empty;
    }

    public class DashboardDto
    {
        public int TotalEmployees { get; set; }
        public int ActiveEmployees { get; set; }
        public int TotalClients { get; set; }
        public int TotalTasks { get; set; }
        public int PendingTasks { get; set; }
        public int InProgressTasks { get; set; }
        public int CompletedTasks { get; set; }
        public double AverageRating { get; set; }
        public List<DepartmentStat> DepartmentStats { get; set; } = new();
        public List<TaskStatusStat> TaskStatusStats { get; set; } = new();
    }

    public class DepartmentStat
    {
        public string Department { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class TaskStatusStat
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
