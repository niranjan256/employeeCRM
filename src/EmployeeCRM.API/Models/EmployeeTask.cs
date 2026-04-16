namespace EmployeeCRM.API.Models
{
    public class EmployeeTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int AssignedToEmployeeId { get; set; }
        public Employee AssignedTo { get; set; } = null!;
        public int AssignedByEmployeeId { get; set; }
        public Employee AssignedBy { get; set; } = null!;
        public string Status { get; set; } = "Pending";
        public string Priority { get; set; } = "Medium";
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedDate { get; set; }
    }
}
