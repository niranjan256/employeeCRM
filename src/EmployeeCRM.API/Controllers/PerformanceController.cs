using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PerformanceController : ControllerBase
    {
        private readonly IPerformanceService _service;
        public PerformanceController(IPerformanceService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("employee/{employeeId:int}")]
        public async Task<IActionResult> GetByEmployee(int employeeId) =>
            Ok(await _service.GetByEmployeeAsync(employeeId));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var review = await _service.GetByIdAsync(id);
            return review == null ? NotFound(new { message = $"Review {id} not found" }) : Ok(review);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] CreatePerformanceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound(new { message = $"Review {id} not found" });
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard() =>
            Ok(await _service.GetDashboardAsync());
    }
}
