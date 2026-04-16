using EmployeeCRM.API.Data;
using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRM.API.Services
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        public ReportService(AppDbContext context) => _context = context;

        public async Task<List<EmployeeSummaryReport>> GetEmployeeSummaryAsync()
        {
            var employees = await _context.Employees
                .Include(e => e.AssignedTasks)
                .Include(e => e.EmployeeClients)
                .Include(e => e.PerformanceReviews)
                .AsNoTracking()
                .Where(e => e.IsActive)
                .ToListAsync();

            return employees.Select(e => new EmployeeSummaryReport
            {
                EmployeeId = e.Id,
                EmployeeName = $"{e.FirstName} {e.LastName}",
                Department = e.Department,
                Designation = e.Designation,
                TotalTasksAssigned = e.AssignedTasks.Count,
                CompletedTasks = e.AssignedTasks.Count(t => t.Status == "Completed"),
                AssignedClients = e.EmployeeClients.Count,
                AverageRating = e.PerformanceReviews.Any()
                    ? Math.Round(e.PerformanceReviews.Average(r => r.Rating), 2) : 0
            }).ToList();
        }

        public async Task<List<TaskSummaryReport>> GetTaskSummaryAsync()
        {
            var tasks = await _context.EmployeeTasks.AsNoTracking().ToListAsync();
            var now = DateTime.UtcNow;

            return tasks
                .GroupBy(t => t.Status)
                .Select(g => new TaskSummaryReport
                {
                    Status = g.Key,
                    Count = g.Count(),
                    OverdueTasks = g.Count(t => t.DueDate < now && t.Status != "Completed")
                }).ToList();
        }

        public async Task<List<ClientEngagementReport>> GetClientEngagementAsync()
        {
            var clients = await _context.Clients
                .Include(c => c.EmployeeClients)
                .AsNoTracking()
                .ToListAsync();

            return clients.Select(c => new ClientEngagementReport
            {
                ClientId = c.Id,
                CompanyName = c.CompanyName,
                Industry = c.Industry,
                AssignedEmployees = c.EmployeeClients.Count,
                CreatedDate = c.CreatedDate
            }).OrderByDescending(c => c.AssignedEmployees).ToList();
        }
    }
}
