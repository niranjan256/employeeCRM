using EmployeeCRM.API.Data;
using EmployeeCRM.API.Models;
using EmployeeCRM.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRM.API.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly AppDbContext _context;
        public ClientRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Client>> GetAllAsync(string? search = null)
        {
            var query = _context.Clients.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c =>
                    c.CompanyName.Contains(search) ||
                    c.ContactPerson.Contains(search) ||
                    c.Industry.Contains(search));
            return await query.OrderBy(c => c.CompanyName).ToListAsync();
        }

        public async Task<Client?> GetByIdAsync(int id) =>
            await _context.Clients
                .Include(c => c.EmployeeClients).ThenInclude(ec => ec.Employee)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Client> CreateAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateAsync(Client client)
        {
            _context.Clients.Update(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null) return false;
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EmployeeClient> AssignClientAsync(int employeeId, int clientId)
        {
            var existing = await _context.EmployeeClients
                .FirstOrDefaultAsync(ec => ec.EmployeeId == employeeId && ec.ClientId == clientId);
            if (existing != null) return existing;

            var assignment = new EmployeeClient { EmployeeId = employeeId, ClientId = clientId, AssignedDate = DateTime.UtcNow, Status = "Active" };
            _context.EmployeeClients.Add(assignment);
            await _context.SaveChangesAsync();
            return assignment;
        }

        public async Task<IEnumerable<EmployeeClient>> GetAssignmentsByEmployeeAsync(int employeeId) =>
            await _context.EmployeeClients
                .Include(ec => ec.Client)
                .AsNoTracking()
                .Where(ec => ec.EmployeeId == employeeId)
                .ToListAsync();

        public async Task<IEnumerable<EmployeeClient>> GetClientHistoryAsync(int clientId) =>
            await _context.EmployeeClients
                .Include(ec => ec.Employee)
                .AsNoTracking()
                .Where(ec => ec.ClientId == clientId)
                .OrderByDescending(ec => ec.AssignedDate)
                .ToListAsync();
    }
}
