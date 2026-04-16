namespace EmployeeCRM.API.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public DateTime DateOfJoining { get; set; }
        public bool IsActive { get; set; } = true;

        // Navigation
        public AppUser? AppUser { get; set; }
        public ICollection<EmployeeClient> EmployeeClients { get; set; } = new List<EmployeeClient>();
        public ICollection<EmployeeTask> AssignedTasks { get; set; } = new List<EmployeeTask>();
        public ICollection<EmployeeTask> CreatedTasks { get; set; } = new List<EmployeeTask>();
        public ICollection<PerformanceReview> PerformanceReviews { get; set; } = new List<PerformanceReview>();
        public ICollection<PerformanceReview> ReviewsGiven { get; set; } = new List<PerformanceReview>();
    }
}
