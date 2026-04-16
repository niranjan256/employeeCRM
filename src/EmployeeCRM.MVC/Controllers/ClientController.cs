using EmployeeCRM.MVC.Services;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.MVC.Controllers
{
    public class ClientController : Controller
    {
        private readonly ClientApiService _clientService;
        private readonly EmployeeApiService _employeeService;

        public ClientController(ClientApiService clientService, EmployeeApiService employeeService)
        {
            _clientService = clientService;
            _employeeService = employeeService;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetString("JwtToken") != null;
        private string UserRole => HttpContext.Session.GetString("Role") ?? "";

        public async Task<IActionResult> Index(string? search = null)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var clients = await _clientService.GetAllAsync(search);
            ViewBag.Search = search;
            return View(clients);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound();
            var history = await _clientService.GetClientHistoryAsync(id);
            ViewBag.History = history;
            return View(client);
        }

        public IActionResult Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid();
            return View(new CreateClientDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClientDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var result = await _clientService.CreateAsync(dto);
            if (result == null) { ModelState.AddModelError("", "Failed to create client."); return View(dto); }
            TempData["Success"] = $"Client {result.CompanyName} created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid();
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound();
            var dto = new UpdateClientDto
            {
                CompanyName = client.CompanyName, ContactPerson = client.ContactPerson,
                Email = client.Email, Phone = client.Phone, Industry = client.Industry
            };
            ViewBag.ClientId = id;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateClientDto dto)
        {
            if (!ModelState.IsValid) { ViewBag.ClientId = id; return View(dto); }
            var success = await _clientService.UpdateAsync(id, dto);
            if (!success) { ModelState.AddModelError("", "Failed to update client."); ViewBag.ClientId = id; return View(dto); }
            TempData["Success"] = "Client updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole != "Admin") return Forbid();
            var client = await _clientService.GetByIdAsync(id);
            if (client == null) return NotFound();
            return View(client);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _clientService.DeleteAsync(id);
            TempData["Success"] = "Client deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Assign()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid();
            ViewBag.Employees = await _employeeService.GetAllAsync(isActive: true);
            ViewBag.Clients = await _clientService.GetAllAsync();
            return View(new AssignClientDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(AssignClientDto dto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees = await _employeeService.GetAllAsync(isActive: true);
                ViewBag.Clients = await _clientService.GetAllAsync();
                return View(dto);
            }
            var result = await _clientService.AssignClientAsync(dto);
            if (result == null) { ModelState.AddModelError("", "Assignment failed."); }
            else TempData["Success"] = "Client assigned to employee successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
