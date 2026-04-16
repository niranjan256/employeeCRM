using EmployeeCRM.API.Data;
using EmployeeCRM.API.Models;
using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmployeeCRM.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.AppUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Username == dto.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;

            var token = GenerateJwt(user);
            return new AuthResponseDto
            {
                Token = token,
                Username = user.Username,
                Role = user.Role,
                UserId = user.Id,
                EmployeeId = user.EmployeeId,
                Expiry = DateTime.UtcNow.AddHours(8)
            };
        }

        public async Task<bool> RegisterAsync(RegisterDto dto)
        {
            if (await _context.AppUsers.AnyAsync(u => u.Username == dto.Username))
                return false;

            var user = new AppUser
            {
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                EmployeeId = dto.EmployeeId
            };
            _context.AppUsers.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private string GenerateJwt(AppUser user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "EmployeeCRM_DefaultSecretKey_2026!"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("EmployeeId", user.EmployeeId?.ToString() ?? "")
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"] ?? "EmployeeCRM.API",
                audience: _config["Jwt:Audience"] ?? "EmployeeCRM.MVC",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
