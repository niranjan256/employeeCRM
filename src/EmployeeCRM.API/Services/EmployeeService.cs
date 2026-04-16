using EmployeeCRM.API.Models;
using EmployeeCRM.API.Repositories.Interfaces;
using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repo;
        public EmployeeService(IEmployeeRepository repo) => _repo = repo;

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync(string? search = null, string? department = null, bool? isActive = null)
        {
            var employees = await _repo.GetAllAsync(search, department, isActive);
            return employees.Select(MapToDto);
        }

        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var emp = await _repo.GetByIdAsync(id);
            return emp == null ? null : MapToDto(emp);
        }

        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            var emp = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Department = dto.Department,
                Designation = dto.Designation,
                DateOfJoining = dto.DateOfJoining,
                IsActive = true
            };
            var created = await _repo.CreateAsync(emp);
            return MapToDto(created);
        }

        public async Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var emp = await _repo.GetByIdAsync(id);
            if (emp == null) return null;

            // Reload tracked entity
            var tracked = new Employee
            {
                Id = id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Department = dto.Department,
                Designation = dto.Designation,
                DateOfJoining = dto.DateOfJoining,
                IsActive = dto.IsActive
            };
            var updated = await _repo.UpdateAsync(tracked);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        private static EmployeeDto MapToDto(Employee e) => new()
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            Department = e.Department,
            Designation = e.Designation,
            DateOfJoining = e.DateOfJoining,
            IsActive = e.IsActive
        };
    }
}
