using EmployeeCRM.API.Data;
using EmployeeCRM.API.Models;
using EmployeeCRM.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRM.API.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly AppDbContext _context;
        public EmployeeRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<Employee>> GetAllAsync(string? search = null, string? department = null, bool? isActive = null)
        {
            var query = _context.Employees.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(e =>
                    e.FirstName.Contains(search) ||
                    e.LastName.Contains(search) ||
                    e.Email.Contains(search) ||
                    e.Designation.Contains(search));

            if (!string.IsNullOrWhiteSpace(department))
                query = query.Where(e => e.Department == department);

            if (isActive.HasValue)
                query = query.Where(e => e.IsActive == isActive.Value);

            return await query.OrderBy(e => e.FirstName).ToListAsync();
        }

        public async Task<Employee?> GetByIdAsync(int id) =>
            await _context.Employees
                .Include(e => e.EmployeeClients).ThenInclude(ec => ec.Client)
                .Include(e => e.AssignedTasks)
                .Include(e => e.PerformanceReviews)
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Employee?> GetByEmailAsync(string email) =>
            await _context.Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Email == email);

        public async Task<Employee> CreateAsync(Employee employee)
        {
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> UpdateAsync(Employee employee)
        {
            _context.Employees.Update(employee);
            await _context.SaveChangesAsync();
            return employee;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var emp = await _context.Employees.FindAsync(id);
            if (emp == null) return false;
            emp.IsActive = false;   // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _context.Employees.AnyAsync(e => e.Id == id);
    }
}
