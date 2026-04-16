using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _service;
        public EmployeesController(IEmployeeService service) => _service = service;

        /// <summary>Get all employees with optional search/filter</summary>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? search = null,
            [FromQuery] string? department = null,
            [FromQuery] bool? isActive = null)
        {
            var employees = await _service.GetAllAsync(search, department, isActive);
            return Ok(employees);
        }

        /// <summary>Get employee by ID</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var emp = await _service.GetByIdAsync(id);
            if (emp == null) return NotFound(new { message = $"Employee {id} not found" });
            return Ok(emp);
        }

        /// <summary>Create new employee</summary>
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>Update employee</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound(new { message = $"Employee {id} not found" });
            return Ok(updated);
        }

        /// <summary>Soft-delete employee</summary>
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = $"Employee {id} not found" });
            return NoContent();
        }
    }
}
