using EmployeeCRM.API.Models;

namespace EmployeeCRM.API.Repositories.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<EmployeeTask>> GetAllAsync(string? status = null, int? employeeId = null);
        Task<EmployeeTask?> GetByIdAsync(int id);
        Task<EmployeeTask> CreateAsync(EmployeeTask task);
        Task<EmployeeTask> UpdateAsync(EmployeeTask task);
        Task<bool> DeleteAsync(int id);
        Task<EmployeeTask> UpdateStatusAsync(int id, string status);
    }
}
