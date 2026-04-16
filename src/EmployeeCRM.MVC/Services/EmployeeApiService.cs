using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.MVC.Services
{
    public class EmployeeApiService : BaseApiService
    {
        public EmployeeApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor) { }

        public async Task<List<EmployeeDto>> GetAllAsync(string? search = null, string? department = null, bool? isActive = null)
        {
            var url = "employees?";
            if (!string.IsNullOrEmpty(search)) url += $"search={Uri.EscapeDataString(search)}&";
            if (!string.IsNullOrEmpty(department)) url += $"department={Uri.EscapeDataString(department)}&";
            if (isActive.HasValue) url += $"isActive={isActive.Value}&";
            return await GetAsync<List<EmployeeDto>>(url.TrimEnd('&', '?')) ?? new();
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id) =>
            await GetAsync<EmployeeDto>($"employees/{id}");

        public async Task<EmployeeDto?> CreateAsync(CreateEmployeeDto dto) =>
            await PostAsync<EmployeeDto>("employees", dto);

        public async Task<bool> UpdateAsync(int id, UpdateEmployeeDto dto) =>
            await PutAsync($"employees/{id}", dto);

        public async Task<bool> DeleteAsync(int id) =>
            await DeleteAsync($"employees/{id}");
    }
}
