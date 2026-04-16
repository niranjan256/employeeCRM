using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.MVC.Services
{
    public class PerformanceApiService : BaseApiService
    {
        public PerformanceApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor) { }

        public async Task<List<PerformanceDto>> GetAllAsync() =>
            await GetAsync<List<PerformanceDto>>("performance") ?? new();

        public async Task<List<PerformanceDto>> GetByEmployeeAsync(int employeeId) =>
            await GetAsync<List<PerformanceDto>>($"performance/employee/{employeeId}") ?? new();

        public async Task<DashboardDto?> GetDashboardAsync() =>
            await GetAsync<DashboardDto>("performance/dashboard");

        public async Task<PerformanceDto?> CreateAsync(CreatePerformanceDto dto) =>
            await PostAsync<PerformanceDto>("performance", dto);

        public async Task<bool> DeleteAsync(int id) =>
            await DeleteAsync($"performance/{id}");
    }
}
