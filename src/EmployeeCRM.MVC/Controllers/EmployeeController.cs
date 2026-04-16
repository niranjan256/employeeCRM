using EmployeeCRM.MVC.Services;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.MVC.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeApiService _employeeService;

        public EmployeeController(EmployeeApiService employeeService) =>
            _employeeService = employeeService;

        private bool IsLoggedIn() => HttpContext.Session.GetString("JwtToken") != null;
        private string UserRole => HttpContext.Session.GetString("Role") ?? "";

        public async Task<IActionResult> Index(string? search = null, string? department = null, bool? isActive = null)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var employees = await _employeeService.GetAllAsync(search, department, isActive);
            ViewBag.Search = search;
            ViewBag.Department = department;
            ViewBag.IsActive = isActive;
            return View(employees);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var emp = await _employeeService.GetByIdAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        public IActionResult Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid();
            return View(new CreateEmployeeDto { DateOfJoining = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _employeeService.CreateAsync(dto);
            if (result == null)
            {
                ModelState.AddModelError("", "Failed to create employee. Email may already exist.");
                return View(dto);
            }
            TempData["Success"] = $"Employee {result.FullName} created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid();
            var emp = await _employeeService.GetByIdAsync(id);
            if (emp == null) return NotFound();
            var dto = new UpdateEmployeeDto
            {
                FirstName = emp.FirstName, LastName = emp.LastName, Email = emp.Email,
                Phone = emp.Phone, Department = emp.Department, Designation = emp.Designation,
                DateOfJoining = emp.DateOfJoining, IsActive = emp.IsActive
            };
            ViewBag.EmployeeId = id;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateEmployeeDto dto)
        {
            if (!ModelState.IsValid) { ViewBag.EmployeeId = id; return View(dto); }
            var success = await _employeeService.UpdateAsync(id, dto);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to update employee.");
                ViewBag.EmployeeId = id;
                return View(dto);
            }
            TempData["Success"] = "Employee updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole != "Admin") return Forbid();
            var emp = await _employeeService.GetByIdAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteAsync(id);
            TempData["Success"] = "Employee deactivated successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
