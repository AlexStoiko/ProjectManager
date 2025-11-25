namespace ProjectManager.Backend.Data.Entities
{
    public class Project
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string CustomerCompany { get; set; }
        public string ContractorCompany { get; set; }

        public int Priority { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // project manager (one of employees)
        public int ManagerId { get; set; }
        public Employee Manager { get; set; }

        // employees working on the project (many-to-many)
        public ICollection<ProjectEmployee> ProjectEmployees { get; set; } =
            new List<ProjectEmployee>();

        // uploaded files
        public ICollection<ProjectFile> Files { get; set; } =
            new List<ProjectFile>();
    }
}
