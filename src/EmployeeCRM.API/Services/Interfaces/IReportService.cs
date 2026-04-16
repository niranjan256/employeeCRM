using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<EmployeeSummaryReport>> GetEmployeeSummaryAsync();
        Task<List<TaskSummaryReport>> GetTaskSummaryAsync();
        Task<List<ClientEngagementReport>> GetClientEngagementAsync();
    }
}
