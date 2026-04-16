namespace EmployeeCRM.API.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation
        public ICollection<EmployeeClient> EmployeeClients { get; set; } = new List<EmployeeClient>();
    }
}
