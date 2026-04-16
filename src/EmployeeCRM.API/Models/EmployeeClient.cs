namespace EmployeeCRM.API.Models
{
    public class EmployeeClient
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Active";
    }
}
