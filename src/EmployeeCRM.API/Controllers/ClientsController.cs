using EmployeeCRM.API.Services.Interfaces;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _service;
        public ClientsController(IClientService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search = null) =>
            Ok(await _service.GetAllAsync(search));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _service.GetByIdAsync(id);
            return client == null ? NotFound(new { message = $"Client {id} not found" }) : Ok(client);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClientDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _service.UpdateAsync(id, dto);
            return updated == null ? NotFound(new { message = $"Client {id} not found" }) : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound(new { message = $"Client {id} not found" });
        }

        [HttpPost("assign")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Assign([FromBody] AssignClientDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.AssignClientAsync(dto);
            return Ok(result);
        }

        [HttpGet("{id:int}/history")]
        public async Task<IActionResult> GetHistory(int id) =>
            Ok(await _service.GetClientHistoryAsync(id));
    }
}
