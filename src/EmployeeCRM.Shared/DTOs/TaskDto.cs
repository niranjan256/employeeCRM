using EmployeeCRM.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeeCRM.Shared.DTOs
{
    public class EmployeeTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AssignedToEmployeeId { get; set; }
        public string AssignedToEmployeeName { get; set; } = string.Empty;
        public int AssignedByEmployeeId { get; set; }
        public string AssignedByEmployeeName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Priority { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    public class CreateTaskDto
    {
        [Required] public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required] public int AssignedToEmployeeId { get; set; }
        [Required] public int AssignedByEmployeeId { get; set; }
        public string Priority { get; set; } = "Medium";
        [Required] public DateTime DueDate { get; set; }
    }

    public class UpdateTaskDto
    {
        [Required] public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required] public int AssignedToEmployeeId { get; set; }
        public string Priority { get; set; } = "Medium";
        [Required] public DateTime DueDate { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class UpdateTaskStatusDto
    {
        [Required] public string Status { get; set; } = string.Empty;
    }
}
