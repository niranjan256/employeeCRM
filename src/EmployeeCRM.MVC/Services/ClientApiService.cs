using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.MVC.Services
{
    public class ClientApiService : BaseApiService
    {
        public ClientApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor) { }

        public async Task<List<ClientDto>> GetAllAsync(string? search = null)
        {
            var url = string.IsNullOrEmpty(search) ? "clients" : $"clients?search={Uri.EscapeDataString(search)}";
            return await GetAsync<List<ClientDto>>(url) ?? new();
        }

        public async Task<ClientDto?> GetByIdAsync(int id) =>
            await GetAsync<ClientDto>($"clients/{id}");

        public async Task<ClientDto?> CreateAsync(CreateClientDto dto) =>
            await PostAsync<ClientDto>("clients", dto);

        public async Task<bool> UpdateAsync(int id, UpdateClientDto dto) =>
            await PutAsync($"clients/{id}", dto);

        public async Task<bool> DeleteAsync(int id) =>
            await DeleteAsync($"clients/{id}");

        public async Task<EmployeeClientDto?> AssignClientAsync(AssignClientDto dto) =>
            await PostAsync<EmployeeClientDto>("clients/assign", dto);

        public async Task<List<EmployeeClientDto>> GetClientHistoryAsync(int clientId) =>
            await GetAsync<List<EmployeeClientDto>>($"clients/{clientId}/history") ?? new();
    }
}
