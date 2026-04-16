using EmployeeCRM.MVC.Services;
using EmployeeCRM.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeCRM.MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskApiService _taskService;
        private readonly EmployeeApiService _employeeService;

        public TaskController(TaskApiService taskService, EmployeeApiService employeeService)
        {
            _taskService = taskService;
            _employeeService = employeeService;
        }

        private bool IsLoggedIn() => HttpContext.Session.GetString("JwtToken") != null;
        private string UserRole => HttpContext.Session.GetString("Role") ?? "";
        private int? UserId => HttpContext.Session.GetInt32("UserId");
        private int? EmployeeId => HttpContext.Session.GetInt32("EmployeeId");

        public async Task<IActionResult> Index(string? status = null)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            
            int? filterEmployeeId = UserRole == "Employee" ? EmployeeId : null;
            var tasks = await _taskService.GetAllAsync(status, filterEmployeeId);
            ViewBag.Status = status;
            return View(tasks);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();

            // Employees can only view their own tasks or tasks they assigned (if any)
            if (UserRole == "Employee" && task.AssignedToEmployeeId != EmployeeId && task.AssignedByEmployeeId != EmployeeId)
                return Forbid();

            return View(task);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid();

            var employees = await _employeeService.GetAllAsync(isActive: true);
            ViewBag.Employees = new SelectList(employees, "Id", "FullName");
            
            return View(new CreateTaskDto { DueDate = DateTime.Today.AddDays(7), AssignedByEmployeeId = EmployeeId ?? 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            if (!ModelState.IsValid)
            {
                var employees = await _employeeService.GetAllAsync(isActive: true);
                ViewBag.Employees = new SelectList(employees, "Id", "FullName");
                return View(dto);
            }
            
            dto.AssignedByEmployeeId = EmployeeId ?? 1; // Fallback to a default if admin without an employee profile creating a task

            var result = await _taskService.CreateAsync(dto);
            if (result == null)
            {
                ModelState.AddModelError("", "Failed to create task.");
                var employees = await _employeeService.GetAllAsync(isActive: true);
                ViewBag.Employees = new SelectList(employees, "Id", "FullName");
                return View(dto);
            }
            TempData["Success"] = "Task created successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");

            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();

            // Employees can only edit tasks assigned to them to update status
            // We'll handle this in the view, but here we just pass the DTO
            if (UserRole == "Employee" && task.AssignedToEmployeeId != EmployeeId) return Forbid();

            var dto = new UpdateTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                AssignedToEmployeeId = task.AssignedToEmployeeId,
                Priority = task.Priority,
                DueDate = task.DueDate,
                Status = task.Status
            };

            var employees = await _employeeService.GetAllAsync(isActive: true);
            ViewBag.Employees = new SelectList(employees, "Id", "FullName", task.AssignedToEmployeeId);
            ViewBag.TaskId = id;
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTaskDto dto)
        {
            // If employee, they can only update status. We override other fields just in case.
            if (UserRole == "Employee")
            {
                var existingTask = await _taskService.GetByIdAsync(id);
                if (existingTask == null || existingTask.AssignedToEmployeeId != EmployeeId) return Forbid();

                var statusUpdateResult = await _taskService.UpdateStatusAsync(id, dto.Status);
                if (!statusUpdateResult)
                {
                    ModelState.AddModelError("", "Failed to update task status.");
                    ViewBag.TaskId = id;
                    // Re-populate select lists for the view structure if needed
                    var employees = await _employeeService.GetAllAsync(isActive: true);
                    ViewBag.Employees = new SelectList(employees, "Id", "FullName", existingTask.AssignedToEmployeeId);
                    return View(dto);
                }
                TempData["Success"] = "Task status updated successfully.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TaskId = id;
                var employees = await _employeeService.GetAllAsync(isActive: true);
                ViewBag.Employees = new SelectList(employees, "Id", "FullName", dto.AssignedToEmployeeId);
                return View(dto);
            }

            var success = await _taskService.UpdateAsync(id, dto);
            if (!success)
            {
                ModelState.AddModelError("", "Failed to update task.");
                ViewBag.TaskId = id;
                var employees = await _employeeService.GetAllAsync(isActive: true);
                ViewBag.Employees = new SelectList(employees, "Id", "FullName", dto.AssignedToEmployeeId);
                return View(dto);
            }
            TempData["Success"] = "Task updated successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Account");
            if (UserRole == "Employee") return Forbid();

            var task = await _taskService.GetByIdAsync(id);
            if (task == null) return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _taskService.DeleteAsync(id);
            TempData["Success"] = "Task deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
