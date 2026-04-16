using EmployeeCRM.API.Data;
using EmployeeCRM.API.Models;
using EmployeeCRM.API.Repositories.Interfaces;
using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRM.API.Services
{
    public class PerformanceService : IPerformanceService
    {
        private readonly IPerformanceRepository _repo;
        private readonly AppDbContext _context;
        public PerformanceService(IPerformanceRepository repo, AppDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        public async Task<IEnumerable<PerformanceDto>> GetAllAsync()
        {
            var reviews = await _repo.GetAllAsync();
            return reviews.Select(MapToDto);
        }

        public async Task<IEnumerable<PerformanceDto>> GetByEmployeeAsync(int employeeId)
        {
            var reviews = await _repo.GetByEmployeeAsync(employeeId);
            return reviews.Select(MapToDto);
        }

        public async Task<PerformanceDto?> GetByIdAsync(int id)
        {
            var r = await _repo.GetByIdAsync(id);
            return r == null ? null : MapToDto(r);
        }

        public async Task<PerformanceDto> CreateAsync(CreatePerformanceDto dto)
        {
            var review = new PerformanceReview
            {
                EmployeeId = dto.EmployeeId,
                ReviewerId = dto.ReviewerId,
                Rating = dto.Rating,
                Comments = dto.Comments,
                ReviewPeriod = dto.ReviewPeriod,
                ReviewDate = DateTime.UtcNow
            };
            var created = await _repo.CreateAsync(review);
            return MapToDto(created);
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<DashboardDto> GetDashboardAsync()
        {
            var employees = await _context.Employees.AsNoTracking().ToListAsync();
            var clients = await _context.Clients.AsNoTracking().ToListAsync();
            var tasks = await _context.EmployeeTasks.AsNoTracking().ToListAsync();
            var reviews = await _context.PerformanceReviews.AsNoTracking().ToListAsync();

            var deptStats = employees
                .GroupBy(e => e.Department)
                .Select(g => new DepartmentStat { Department = g.Key, Count = g.Count() })
                .ToList();

            var taskStats = tasks
                .GroupBy(t => t.Status)
                .Select(g => new TaskStatusStat { Status = g.Key, Count = g.Count() })
                .ToList();

            return new DashboardDto
            {
                TotalEmployees = employees.Count,
                ActiveEmployees = employees.Count(e => e.IsActive),
                TotalClients = clients.Count,
                TotalTasks = tasks.Count,
                PendingTasks = tasks.Count(t => t.Status == "Pending"),
                InProgressTasks = tasks.Count(t => t.Status == "InProgress"),
                CompletedTasks = tasks.Count(t => t.Status == "Completed"),
                AverageRating = reviews.Any() ? Math.Round(reviews.Average(r => r.Rating), 2) : 0,
                DepartmentStats = deptStats,
                TaskStatusStats = taskStats
            };
        }

        private static PerformanceDto MapToDto(PerformanceReview r) => new()
        {
            Id = r.Id,
            EmployeeId = r.EmployeeId,
            EmployeeName = r.Employee != null ? $"{r.Employee.FirstName} {r.Employee.LastName}" : string.Empty,
            ReviewerId = r.ReviewerId,
            ReviewerName = r.Reviewer != null ? $"{r.Reviewer.FirstName} {r.Reviewer.LastName}" : string.Empty,
            Rating = r.Rating,
            Comments = r.Comments,
            ReviewPeriod = r.ReviewPeriod,
            ReviewDate = r.ReviewDate
        };
    }
}
