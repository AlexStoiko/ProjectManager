namespace ProjectManager.Backend.Data.Entities
{
    // Position (job title) for employees
    public class Position
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // One-to-many: Position -> Employees
        public List<Employee> Employees { get; set; } = new List<Employee>();
    }
}
