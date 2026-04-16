using EmployeeCRM.MVC.Services;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCRM.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthApiService _authService;

        public AccountController(AuthApiService authService) => _authService = authService;

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (HttpContext.Session.GetString("JwtToken") != null)
                return RedirectToAction("Index", "Home");
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto, string? returnUrl = null)
        {
            if (!ModelState.IsValid) return View(dto);

            var result = await _authService.LoginAsync(dto);
            if (result == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(dto);
            }

            HttpContext.Session.SetString("JwtToken", result.Token);
            HttpContext.Session.SetString("Username", result.Username);
            HttpContext.Session.SetString("Role", result.Role);
            HttpContext.Session.SetInt32("UserId", result.UserId);
            if (result.EmployeeId.HasValue)
                HttpContext.Session.SetInt32("EmployeeId", result.EmployeeId.Value);

            return Redirect(returnUrl ?? "/");
        }

        [HttpGet]
        public IActionResult Register()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role != "Admin") return RedirectToAction("Login");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            if (!ModelState.IsValid) return View(dto);
            var success = await _authService.RegisterAsync(dto);
            if (!success)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View(dto);
            }
            TempData["Success"] = "User registered successfully.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
