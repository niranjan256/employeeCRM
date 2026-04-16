using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services.Interfaces
{
    public interface IPerformanceService
    {
        Task<IEnumerable<PerformanceDto>> GetAllAsync();
        Task<IEnumerable<PerformanceDto>> GetByEmployeeAsync(int employeeId);
        Task<PerformanceDto?> GetByIdAsync(int id);
        Task<PerformanceDto> CreateAsync(CreatePerformanceDto dto);
        Task<bool> DeleteAsync(int id);
        Task<DashboardDto> GetDashboardAsync();
    }
}
