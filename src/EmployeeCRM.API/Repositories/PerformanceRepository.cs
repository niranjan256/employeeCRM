using EmployeeCRM.API.Data;
using EmployeeCRM.API.Models;
using EmployeeCRM.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRM.API.Repositories
{
    public class PerformanceRepository : IPerformanceRepository
    {
        private readonly AppDbContext _context;
        public PerformanceRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<PerformanceReview>> GetAllAsync() =>
            await _context.PerformanceReviews
                .Include(r => r.Employee)
                .Include(r => r.Reviewer)
                .AsNoTracking()
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();

        public async Task<IEnumerable<PerformanceReview>> GetByEmployeeAsync(int employeeId) =>
            await _context.PerformanceReviews
                .Include(r => r.Reviewer)
                .AsNoTracking()
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.ReviewDate)
                .ToListAsync();

        public async Task<PerformanceReview?> GetByIdAsync(int id) =>
            await _context.PerformanceReviews
                .Include(r => r.Employee)
                .Include(r => r.Reviewer)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);

        public async Task<PerformanceReview> CreateAsync(PerformanceReview review)
        {
            _context.PerformanceReviews.Add(review);
            await _context.SaveChangesAsync();
            return review;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.PerformanceReviews.FindAsync(id);
            if (review == null) return false;
            _context.PerformanceReviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
