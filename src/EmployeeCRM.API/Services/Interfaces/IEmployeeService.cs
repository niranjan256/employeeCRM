using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<IEnumerable<EmployeeDto>> GetAllAsync(string? search = null, string? department = null, bool? isActive = null);
        Task<EmployeeDto?> GetByIdAsync(int id);
        Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto);
        Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
