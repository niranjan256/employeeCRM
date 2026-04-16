using EmployeeCRM.API.Models;

namespace EmployeeCRM.API.Repositories.Interfaces
{
    public interface IPerformanceRepository
    {
        Task<IEnumerable<PerformanceReview>> GetAllAsync();
        Task<IEnumerable<PerformanceReview>> GetByEmployeeAsync(int employeeId);
        Task<PerformanceReview?> GetByIdAsync(int id);
        Task<PerformanceReview> CreateAsync(PerformanceReview review);
        Task<bool> DeleteAsync(int id);
    }
}
