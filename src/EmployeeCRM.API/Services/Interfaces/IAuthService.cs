using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<bool> RegisterAsync(RegisterDto dto);
    }
}
