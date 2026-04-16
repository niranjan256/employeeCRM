using EmployeeCRM.API.Models;
using EmployeeCRM.API.Repositories.Interfaces;
using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repo;
        public ClientService(IClientRepository repo) => _repo = repo;

        public async Task<IEnumerable<ClientDto>> GetAllAsync(string? search = null)
        {
            var clients = await _repo.GetAllAsync(search);
            return clients.Select(MapToDto);
        }

        public async Task<ClientDto?> GetByIdAsync(int id)
        {
            var c = await _repo.GetByIdAsync(id);
            return c == null ? null : MapToDto(c);
        }

        public async Task<ClientDto> CreateAsync(CreateClientDto dto)
        {
            var client = new Client
            {
                CompanyName = dto.CompanyName,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                Industry = dto.Industry,
                CreatedDate = DateTime.UtcNow
            };
            var created = await _repo.CreateAsync(client);
            return MapToDto(created);
        }

        public async Task<ClientDto?> UpdateAsync(int id, UpdateClientDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            var client = new Client
            {
                Id = id,
                CompanyName = dto.CompanyName,
                ContactPerson = dto.ContactPerson,
                Email = dto.Email,
                Phone = dto.Phone,
                Industry = dto.Industry,
                CreatedDate = existing.CreatedDate
            };
            var updated = await _repo.UpdateAsync(client);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<EmployeeClientDto> AssignClientAsync(AssignClientDto dto)
        {
            var ec = await _repo.AssignClientAsync(dto.EmployeeId, dto.ClientId);
            return new EmployeeClientDto
            {
                Id = ec.Id,
                EmployeeId = ec.EmployeeId,
                ClientId = ec.ClientId,
                AssignedDate = ec.AssignedDate,
                Status = ec.Status
            };
        }

        public async Task<IEnumerable<EmployeeClientDto>> GetClientHistoryAsync(int clientId)
        {
            var history = await _repo.GetClientHistoryAsync(clientId);
            return history.Select(ec => new EmployeeClientDto
            {
                Id = ec.Id,
                EmployeeId = ec.EmployeeId,
                EmployeeName = $"{ec.Employee?.FirstName} {ec.Employee?.LastName}",
                ClientId = ec.ClientId,
                CompanyName = ec.Client?.CompanyName ?? string.Empty,
                AssignedDate = ec.AssignedDate,
                Status = ec.Status
            });
        }

        public async Task<IEnumerable<EmployeeClientDto>> GetAssignmentsByEmployeeAsync(int employeeId)
        {
            var assignments = await _repo.GetAssignmentsByEmployeeAsync(employeeId);
            return assignments.Select(ec => new EmployeeClientDto
            {
                Id = ec.Id,
                EmployeeId = ec.EmployeeId,
                ClientId = ec.ClientId,
                CompanyName = ec.Client?.CompanyName ?? string.Empty,
                AssignedDate = ec.AssignedDate,
                Status = ec.Status
            });
        }

        private static ClientDto MapToDto(Client c) => new()
        {
            Id = c.Id,
            CompanyName = c.CompanyName,
            ContactPerson = c.ContactPerson,
            Email = c.Email,
            Phone = c.Phone,
            Industry = c.Industry,
            CreatedDate = c.CreatedDate
        };
    }
}
