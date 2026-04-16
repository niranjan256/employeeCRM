using BCrypt.Net;
using EmployeeCRM.API.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRM.API.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();

            if (context.Employees.Any()) return;

            // Seed Employees
            var employees = new List<Employee>
            {
                new() { FirstName = "Pavan", LastName = "Kalyan", Email = "pavan.kalyan@crm.com", Phone = "9876543210", Department = "Engineering", Designation = "Senior Developer", DateOfJoining = new DateTime(2021, 3, 15), IsActive = true },
                new() { FirstName = "Arjun", LastName = "Reddy", Email = "arjun.reddy@crm.com", Phone = "9876543211", Department = "Engineering", Designation = "Developer", DateOfJoining = new DateTime(2022, 6, 1), IsActive = true },
                new() { FirstName = "Priya", LastName = "Sharma", Email = "priya.sharma@crm.com", Phone = "9876543212", Department = "HR", Designation = "HR Manager", DateOfJoining = new DateTime(2020, 1, 10), IsActive = true },
                new() { FirstName = "Ravi", LastName = "Kumar", Email = "ravi.kumar@crm.com", Phone = "9876543213", Department = "Sales", Designation = "Sales Manager", DateOfJoining = new DateTime(2019, 7, 20), IsActive = true },
                new() { FirstName = "Anita", LastName = "Singh", Email = "anita.singh@crm.com", Phone = "9876543214", Department = "Marketing", Designation = "Marketing Analyst", DateOfJoining = new DateTime(2023, 2, 1), IsActive = true },
                new() { FirstName = "Vikram", LastName = "Nair", Email = "vikram.nair@crm.com", Phone = "9876543215", Department = "Engineering", Designation = "QA Engineer", DateOfJoining = new DateTime(2022, 9, 15), IsActive = true },
            };
            context.Employees.AddRange(employees);
            context.SaveChanges();

            // Seed AppUsers (admin, manager, employee)
            var users = new List<AppUser>
            {
                new() { Username = "admin", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), Role = "Admin" },
                new() { Username = "ravi.kumar", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Manager@123"), Role = "Manager", EmployeeId = employees[3].Id },
                new() { Username = "pavan.kalyan", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee@123"), Role = "Employee", EmployeeId = employees[0].Id },
                new() { Username = "arjun.reddy", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Employee@123"), Role = "Employee", EmployeeId = employees[1].Id },
            };
            context.AppUsers.AddRange(users);
            context.SaveChanges();

            // Seed Clients
            var clients = new List<Client>
            {
                new() { CompanyName = "TechCorp Solutions", ContactPerson = "Suresh Menon", Email = "suresh@techcorp.com", Phone = "8001234567", Industry = "Technology", CreatedDate = DateTime.UtcNow.AddMonths(-6) },
                new() { CompanyName = "FinEdge Ltd", ContactPerson = "Meera Pillai", Email = "meera@finedge.com", Phone = "8001234568", Industry = "Finance", CreatedDate = DateTime.UtcNow.AddMonths(-4) },
                new() { CompanyName = "HealthFirst Inc", ContactPerson = "Dr. Ramesh Rao", Email = "ramesh@healthfirst.com", Phone = "8001234569", Industry = "Healthcare", CreatedDate = DateTime.UtcNow.AddMonths(-3) },
                new() { CompanyName = "EduBridge", ContactPerson = "Kavita Joshi", Email = "kavita@edubridge.com", Phone = "8001234570", Industry = "Education", CreatedDate = DateTime.UtcNow.AddMonths(-2) },
                new() { CompanyName = "RetailZone", ContactPerson = "Arun Bose", Email = "arun@retailzone.com", Phone = "8001234571", Industry = "Retail", CreatedDate = DateTime.UtcNow.AddMonths(-1) },
            };
            context.Clients.AddRange(clients);
            context.SaveChanges();

            // Seed EmployeeClient assignments
            var assignments = new List<EmployeeClient>
            {
                new() { EmployeeId = employees[0].Id, ClientId = clients[0].Id, AssignedDate = DateTime.UtcNow.AddMonths(-5), Status = "Active" },
                new() { EmployeeId = employees[0].Id, ClientId = clients[1].Id, AssignedDate = DateTime.UtcNow.AddMonths(-3), Status = "Active" },
                new() { EmployeeId = employees[1].Id, ClientId = clients[2].Id, AssignedDate = DateTime.UtcNow.AddMonths(-2), Status = "Active" },
                new() { EmployeeId = employees[3].Id, ClientId = clients[3].Id, AssignedDate = DateTime.UtcNow.AddMonths(-1), Status = "Active" },
                new() { EmployeeId = employees[4].Id, ClientId = clients[4].Id, AssignedDate = DateTime.UtcNow.AddDays(-15), Status = "Active" },
            };
            context.EmployeeClients.AddRange(assignments);
            context.SaveChanges();

            // Seed Tasks
            var tasks = new List<EmployeeTask>
            {
                new() { Title = "Develop Login Module", Description = "Build secure login with JWT", AssignedToEmployeeId = employees[0].Id, AssignedByEmployeeId = employees[3].Id, Status = "Completed", Priority = "High", DueDate = DateTime.UtcNow.AddDays(-10), CreatedDate = DateTime.UtcNow.AddDays(-30), CompletedDate = DateTime.UtcNow.AddDays(-12) },
                new() { Title = "API Integration Testing", Description = "Test all REST endpoints", AssignedToEmployeeId = employees[5].Id, AssignedByEmployeeId = employees[3].Id, Status = "InProgress", Priority = "High", DueDate = DateTime.UtcNow.AddDays(5), CreatedDate = DateTime.UtcNow.AddDays(-15) },
                new() { Title = "Client Onboarding - TechCorp", Description = "Prepare onboarding documents", AssignedToEmployeeId = employees[1].Id, AssignedByEmployeeId = employees[3].Id, Status = "Pending", Priority = "Medium", DueDate = DateTime.UtcNow.AddDays(7), CreatedDate = DateTime.UtcNow.AddDays(-5) },
                new() { Title = "Monthly HR Report", Description = "Compile monthly employee report", AssignedToEmployeeId = employees[2].Id, AssignedByEmployeeId = employees[2].Id, Status = "Pending", Priority = "Low", DueDate = DateTime.UtcNow.AddDays(3), CreatedDate = DateTime.UtcNow.AddDays(-2) },
                new() { Title = "Marketing Campaign Q2", Description = "Plan and execute Q2 digital marketing", AssignedToEmployeeId = employees[4].Id, AssignedByEmployeeId = employees[3].Id, Status = "InProgress", Priority = "Critical", DueDate = DateTime.UtcNow.AddDays(14), CreatedDate = DateTime.UtcNow.AddDays(-7) },
                new() { Title = "Database Optimization", Description = "Optimize slow LINQ queries", AssignedToEmployeeId = employees[0].Id, AssignedByEmployeeId = employees[3].Id, Status = "Pending", Priority = "Medium", DueDate = DateTime.UtcNow.AddDays(10), CreatedDate = DateTime.UtcNow.AddDays(-1) },
            };
            context.EmployeeTasks.AddRange(tasks);
            context.SaveChanges();

            // Seed Performance Reviews
            var reviews = new List<PerformanceReview>
            {
                new() { EmployeeId = employees[0].Id, ReviewerId = employees[3].Id, Rating = 5, Comments = "Outstanding performance, delivered ahead of schedule.", ReviewPeriod = "Q1 2026", ReviewDate = DateTime.UtcNow.AddMonths(-1) },
                new() { EmployeeId = employees[1].Id, ReviewerId = employees[3].Id, Rating = 4, Comments = "Very good work. Needs improvement on documentation.", ReviewPeriod = "Q1 2026", ReviewDate = DateTime.UtcNow.AddMonths(-1) },
                new() { EmployeeId = employees[2].Id, ReviewerId = employees[2].Id, Rating = 4, Comments = "Excellent HR management and employee relations.", ReviewPeriod = "Q1 2026", ReviewDate = DateTime.UtcNow.AddMonths(-1) },
                new() { EmployeeId = employees[4].Id, ReviewerId = employees[3].Id, Rating = 3, Comments = "Good effort but needs to improve delivery speed.", ReviewPeriod = "Q1 2026", ReviewDate = DateTime.UtcNow.AddMonths(-1) },
                new() { EmployeeId = employees[5].Id, ReviewerId = employees[3].Id, Rating = 4, Comments = "Solid QA work. Found critical bugs in time.", ReviewPeriod = "Q1 2026", ReviewDate = DateTime.UtcNow.AddMonths(-1) },
            };
            context.PerformanceReviews.AddRange(reviews);
            context.SaveChanges();
        }
    }
}
