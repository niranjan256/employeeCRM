using EmployeeCRM.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace EmployeeCRM.Shared.DTOs
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public bool IsActive { get; set; }
        public int? AppUserId { get; set; }
    }

    public class CreateEmployeeDto
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Phone] public string Phone { get; set; } = string.Empty;
        [Required] public string Department { get; set; } = string.Empty;
        [Required] public string Designation { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; } = DateTime.Today;
    }

    public class UpdateEmployeeDto
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Phone] public string Phone { get; set; } = string.Empty;
        [Required] public string Department { get; set; } = string.Empty;
        [Required] public string Designation { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public bool IsActive { get; set; }
    }
}
