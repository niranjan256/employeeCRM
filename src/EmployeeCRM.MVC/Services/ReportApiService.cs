using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.MVC.Services
{
    public class ReportApiService : BaseApiService
    {
        public ReportApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor) { }

        public async Task<List<EmployeeSummaryReport>> GetEmployeeSummaryAsync() =>
            await GetAsync<List<EmployeeSummaryReport>>("reports/employee-summary") ?? new();

        public async Task<List<TaskSummaryReport>> GetTaskSummaryAsync() =>
            await GetAsync<List<TaskSummaryReport>>("reports/task-summary") ?? new();

        public async Task<List<ClientEngagementReport>> GetClientEngagementAsync() =>
            await GetAsync<List<ClientEngagementReport>>("reports/client-engagement") ?? new();
    }
}
