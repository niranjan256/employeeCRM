using EmployeeCRM.API.Data;
using EmployeeCRM.API.Models;
using EmployeeCRM.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRM.API.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;
        public TaskRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<EmployeeTask>> GetAllAsync(string? status = null, int? employeeId = null)
        {
            var query = _context.EmployeeTasks
                .Include(t => t.AssignedTo)
                .Include(t => t.AssignedBy)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(t => t.Status == status);

            if (employeeId.HasValue)
                query = query.Where(t => t.AssignedToEmployeeId == employeeId.Value);

            return await query.OrderByDescending(t => t.CreatedDate).ToListAsync();
        }

        public async Task<EmployeeTask?> GetByIdAsync(int id) =>
            await _context.EmployeeTasks
                .Include(t => t.AssignedTo)
                .Include(t => t.AssignedBy)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<EmployeeTask> CreateAsync(EmployeeTask task)
        {
            _context.EmployeeTasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<EmployeeTask> UpdateAsync(EmployeeTask task)
        {
            _context.EmployeeTasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.EmployeeTasks.FindAsync(id);
            if (task == null) return false;
            _context.EmployeeTasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EmployeeTask> UpdateStatusAsync(int id, string status)
        {
            var task = await _context.EmployeeTasks.FindAsync(id)
                       ?? throw new KeyNotFoundException($"Task {id} not found");
            task.Status = status;
            if (status == "Completed") task.CompletedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return task;
        }
    }
}
