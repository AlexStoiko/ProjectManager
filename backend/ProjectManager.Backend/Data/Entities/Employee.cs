namespace ProjectManager.Backend.Data.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string Email { get; set; }

        // Foreign key to Position
        public int? PositionId { get; set; }
        public Position Position { get; set; }

        // many-to-many relation
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; } =
            new List<ProjectEmployee>();
    }
}
