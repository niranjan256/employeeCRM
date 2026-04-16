using System.ComponentModel.DataAnnotations;

namespace EmployeeCRM.Shared.DTOs
{
    public class ClientDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }

    public class CreateClientDto
    {
        [Required] public string CompanyName { get; set; } = string.Empty;
        [Required] public string ContactPerson { get; set; } = string.Empty;
        [EmailAddress] public string Email { get; set; } = string.Empty;
        [Phone] public string Phone { get; set; } = string.Empty;
        [Required] public string Industry { get; set; } = string.Empty;
    }

    public class UpdateClientDto
    {
        [Required] public string CompanyName { get; set; } = string.Empty;
        [Required] public string ContactPerson { get; set; } = string.Empty;
        [EmailAddress] public string Email { get; set; } = string.Empty;
        [Phone] public string Phone { get; set; } = string.Empty;
        [Required] public string Industry { get; set; } = string.Empty;
    }

    public class AssignClientDto
    {
        [Required] public int EmployeeId { get; set; }
        [Required] public int ClientId { get; set; }
    }

    public class EmployeeClientDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public int ClientId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
