using EmployeeCRM.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly PerformanceApiService _perfService;

        public HomeController(PerformanceApiService perfService) => _perfService = perfService;

        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("JwtToken") == null)
                return RedirectToAction("Login", "Account");

            var dashboard = await _perfService.GetDashboardAsync();
            return View(dashboard);
        }
    }
}
