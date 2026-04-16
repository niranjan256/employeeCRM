using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) => _authService = authService;

        /// <summary>Login and get JWT token</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var response = await _authService.LoginAsync(dto);
            if (response == null) return Unauthorized(new { message = "Invalid username or password" });
            return Ok(response);
        }

        /// <summary>Register a new user (Admin only in production)</summary>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _authService.RegisterAsync(dto);
            if (!success) return Conflict(new { message = "Username already exists" });
            return Ok(new { message = "User registered successfully" });
        }
    }
}
