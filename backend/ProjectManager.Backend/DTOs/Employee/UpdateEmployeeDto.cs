namespace ProjectManager.Backend.Data.DTOs.Employee
{
    public class UpdateEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Email { get; set; }

        public int? PositionId { get; set; }
    }
}
