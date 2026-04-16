using EmployeeCRM.MVC.Services;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeCRM.MVC.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly PerformanceApiService _perfService;
        private readonly EmployeeApiService _employeeService;

        public PerformanceController(PerformanceApiService perfService, EmployeeApiService employeeService)
        {
            _perfService = perfService;
            _employeeService = employeeService;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetString("JwtToken") != null;
        private string UserRole => HttpContext.Session.GetString("Role") ?? "";
        private int? EmployeeId => HttpContext.Session.GetInt32("EmployeeId");

        public async Task<IActionResult> Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            List<PerformanceDto> reviews = new();

            if (UserRole == "Employee")
            {
                if (EmployeeId.HasValue)
                {
                    reviews = await _perfService.GetByEmployeeAsync(EmployeeId.Value);
                }
            }
            else
            {
                 reviews = await _perfService.GetAllAsync();
            }

            return View(reviews);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid();

            var employees = await _employeeService.GetAllAsync(isActive: true);
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");
            
            return View(new CreatePerformanceDto { ReviewPeriod = "Q" + (DateTime.Now.Month / 3 + 1).ToString() + " " + DateTime.Now.Year });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePerformanceDto dto)
        {
            if (!ModelState.IsValid)
            {
                var employees = await _employeeService.GetAllAsync(isActive: true);
                ViewBag.Employees = new SelectList(employees, "Id", "FullName");
                return View(dto);
            }
            
            dto.ReviewerId = EmployeeId ?? 1; // Assuming 1 is default admin if not matched

            var result = await _perfService.CreateAsync(dto);
            if (result == null)
            {
                ModelState.AddModelError("", "Failed to save performance review.");
                var employees = await _employeeService.GetAllAsync(isActive: true);
                ViewBag.Employees = new SelectList(employees, "Id", "FullName");
                return View(dto);
            }

            TempData["Success"] = "Performance review added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole != "Admin") return Forbid();
            var review = await _perfService.GetByIdAsync(id);
            if (review == null) return NotFound();
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _perfService.DeleteAsync(id);
            TempData["Success"] = "Performance review deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
