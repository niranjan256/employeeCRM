using EmployeeCRM.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin,Manager")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;
        public ReportsController(IReportService service) => _service = service;

        [HttpGet("employee-summary")]
        public async Task<IActionResult> GetEmployeeSummary() =>
            Ok(await _service.GetEmployeeSummaryAsync());

        [HttpGet("task-summary")]
        public async Task<IActionResult> GetTaskSummary() =>
            Ok(await _service.GetTaskSummaryAsync());

        [HttpGet("client-engagement")]
        public async Task<IActionResult> GetClientEngagement() =>
            Ok(await _service.GetClientEngagementAsync());
    }
}
