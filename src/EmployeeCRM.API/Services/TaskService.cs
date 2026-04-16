using EmployeeCRM.API.Models;
using EmployeeCRM.API.Repositories.Interfaces;
using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;
        public TaskService(ITaskRepository repo) => _repo = repo;

        public async Task<IEnumerable<EmployeeTaskDto>> GetAllAsync(string? status = null, int? employeeId = null)
        {
            var tasks = await _repo.GetAllAsync(status, employeeId);
            return tasks.Select(MapToDto);
        }

        public async Task<EmployeeTaskDto?> GetByIdAsync(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            return t == null ? null : MapToDto(t);
        }

        public async Task<EmployeeTaskDto> CreateAsync(CreateTaskDto dto)
        {
            var task = new EmployeeTask
            {
                Title = dto.Title,
                Description = dto.Description,
                AssignedToEmployeeId = dto.AssignedToEmployeeId,
                AssignedByEmployeeId = dto.AssignedByEmployeeId,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                Status = "Pending",
                CreatedDate = DateTime.UtcNow
            };
            var created = await _repo.CreateAsync(task);
            return MapToDto(created);
        }

        public async Task<EmployeeTaskDto?> UpdateAsync(int id, UpdateTaskDto dto)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) return null;

            var task = new EmployeeTask
            {
                Id = id,
                Title = dto.Title,
                Description = dto.Description,
                AssignedToEmployeeId = dto.AssignedToEmployeeId,
                AssignedByEmployeeId = existing.AssignedByEmployeeId,
                Priority = dto.Priority,
                DueDate = dto.DueDate,
                Status = dto.Status,
                CreatedDate = existing.CreatedDate,
                CompletedDate = dto.Status == "Completed" ? DateTime.UtcNow : existing.CompletedDate
            };
            var updated = await _repo.UpdateAsync(task);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

        public async Task<EmployeeTaskDto> UpdateStatusAsync(int id, UpdateTaskStatusDto dto)
        {
            var updated = await _repo.UpdateStatusAsync(id, dto.Status);
            return MapToDto(updated);
        }

        private static EmployeeTaskDto MapToDto(EmployeeTask t) => new()
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            AssignedToEmployeeId = t.AssignedToEmployeeId,
            AssignedToEmployeeName = t.AssignedTo != null ? $"{t.AssignedTo.FirstName} {t.AssignedTo.LastName}" : string.Empty,
            AssignedByEmployeeId = t.AssignedByEmployeeId,
            AssignedByEmployeeName = t.AssignedBy != null ? $"{t.AssignedBy.FirstName} {t.AssignedBy.LastName}" : string.Empty,
            Status = t.Status,
            Priority = t.Priority,
            DueDate = t.DueDate,
            CreatedDate = t.CreatedDate,
            CompletedDate = t.CompletedDate
        };
    }
}
