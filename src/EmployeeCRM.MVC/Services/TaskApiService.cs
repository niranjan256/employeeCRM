using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.MVC.Services
{
    public class TaskApiService : BaseApiService
    {
        public TaskApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor) { }

        public async Task<List<EmployeeTaskDto>> GetAllAsync(string? status = null, int? employeeId = null)
        {
            var url = "tasks?";
            if (!string.IsNullOrEmpty(status)) url += $"status={status}&";
            if (employeeId.HasValue) url += $"employeeId={employeeId.Value}&";
            return await GetAsync<List<EmployeeTaskDto>>(url.TrimEnd('&', '?')) ?? new();
        }

        public async Task<EmployeeTaskDto?> GetByIdAsync(int id) =>
            await GetAsync<EmployeeTaskDto>($"tasks/{id}");

        public async Task<EmployeeTaskDto?> CreateAsync(CreateTaskDto dto) =>
            await PostAsync<EmployeeTaskDto>("tasks", dto);

        public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto) =>
            await PutAsync($"tasks/{id}", dto);

        public async Task<bool> UpdateStatusAsync(int id, string status) =>
            await PatchAsync($"tasks/{id}/status", new UpdateTaskStatusDto { Status = status });

        public async Task<bool> DeleteAsync(int id) =>
            await DeleteAsync($"tasks/{id}");
    }
}
