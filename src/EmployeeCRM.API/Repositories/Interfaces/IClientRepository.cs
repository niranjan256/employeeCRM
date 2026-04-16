using EmployeeCRM.API.Models;

namespace EmployeeCRM.API.Repositories.Interfaces
{
    public interface IClientRepository
    {
        Task<IEnumerable<Client>> GetAllAsync(string? search = null);
        Task<Client?> GetByIdAsync(int id);
        Task<Client> CreateAsync(Client client);
        Task<Client> UpdateAsync(Client client);
        Task<bool> DeleteAsync(int id);
        Task<EmployeeClient> AssignClientAsync(int employeeId, int clientId);
        Task<IEnumerable<EmployeeClient>> GetAssignmentsByEmployeeAsync(int employeeId);
        Task<IEnumerable<EmployeeClient>> GetClientHistoryAsync(int clientId);
    }
}
