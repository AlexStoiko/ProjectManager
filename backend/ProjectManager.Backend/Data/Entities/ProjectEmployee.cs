namespace ProjectManager.Backend.Data.Entities
{
    // Many-to-many relation between Projects and Employees
    public class ProjectEmployee
    {
        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
