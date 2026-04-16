using EmployeeCRM.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.MVC.Controllers
{
    public class ReportController : Controller
    {
        private readonly ReportApiService _reportService;

        public ReportController(ReportApiService reportService)
        {
            _reportService = reportService;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetString("JwtToken") != null;
        private string UserRole => HttpContext.Session.GetString("Role") ?? "";

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid(); // Employees cannot view these reports

            var taskSummary = await _reportService.GetTaskSummaryAsync();
            var employeeSummary = await _reportService.GetEmployeeSummaryAsync();
            var clientEngagement = await _reportService.GetClientEngagementAsync();

            ViewBag.TaskSummary = taskSummary;
            ViewBag.EmployeeSummary = employeeSummary;
            ViewBag.ClientEngagement = clientEngagement;

            return View();
        }
    }
}
