namespace EmployeeCRM.Shared.DTOs
{
    public class ReportDto
    {
        public List<EmployeeSummaryReport> EmployeeSummaries { get; set; } = new();
        public List<TaskSummaryReport> TaskSummaries { get; set; } = new();
        public List<ClientEngagementReport> ClientEngagements { get; set; } = new();
    }

    public class EmployeeSummaryReport
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public int TotalTasksAssigned { get; set; }
        public int CompletedTasks { get; set; }
        public int AssignedClients { get; set; }
        public double AverageRating { get; set; }
    }

    public class TaskSummaryReport
    {
        public string Status { get; set; } = string.Empty;
        public int Count { get; set; }
        public int OverdueTasks { get; set; }
    }

    public class ClientEngagementReport
    {
        public int ClientId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public int AssignedEmployees { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
