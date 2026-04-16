using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<EmployeeTaskDto>> GetAllAsync(string? status = null, int? employeeId = null);
        Task<EmployeeTaskDto?> GetByIdAsync(int id);
        Task<EmployeeTaskDto> CreateAsync(CreateTaskDto dto);
        Task<EmployeeTaskDto?> UpdateAsync(int id, UpdateTaskDto dto);
        Task<bool> DeleteAsync(int id);
        Task<EmployeeTaskDto> UpdateStatusAsync(int id, UpdateTaskStatusDto dto);
    }
}
