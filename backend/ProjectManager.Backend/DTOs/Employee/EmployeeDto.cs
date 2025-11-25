namespace ProjectManager.Backend.Data.DTOs.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Email { get; set; }

        public int? PositionId { get; set; }
        public string? PositionName { get; set; }
    }
}
