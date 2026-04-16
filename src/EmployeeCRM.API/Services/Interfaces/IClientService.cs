using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services.Interfaces
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync(string? search = null);
        Task<ClientDto?> GetByIdAsync(int id);
        Task<ClientDto> CreateAsync(CreateClientDto dto);
        Task<ClientDto?> UpdateAsync(int id, UpdateClientDto dto);
        Task<bool> DeleteAsync(int id);
        Task<EmployeeClientDto> AssignClientAsync(AssignClientDto dto);
        Task<IEnumerable<EmployeeClientDto>> GetClientHistoryAsync(int clientId);
        Task<IEnumerable<EmployeeClientDto>> GetAssignmentsByEmployeeAsync(int employeeId);
    }
}
