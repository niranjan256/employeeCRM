using System.ComponentModel.DataAnnotations;

namespace EmployeeCRM.Shared.DTOs
{
    public class LoginDto
    {
        [Required] public string Username { get; set; } = string.Empty;
        [Required] public string Password { get; set; } = string.Empty;
    }

    public class RegisterDto
    {
        [Required] public string Username { get; set; } = string.Empty;
        [Required, MinLength(6)] public string Password { get; set; } = string.Empty;
        [Required] public string Role { get; set; } = "Employee";
        public int? EmployeeId { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public int UserId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime Expiry { get; set; }
    }
}
