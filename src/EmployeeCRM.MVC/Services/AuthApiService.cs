using EmployeeCRM.Shared.DTOs;

namespace EmployeeCRM.MVC.Services
{
    public class AuthApiService : BaseApiService
    {
        public AuthApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
            : base(httpClient, httpContextAccessor) { }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto) =>
            await PostAsync<AuthResponseDto>("auth/login", dto);

        public async Task<bool> RegisterAsync(RegisterDto dto) =>
            await PostAsync<object>("auth/register", dto) != null;
    }
}
