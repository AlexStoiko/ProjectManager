using System;
using System.Collections.Generic;

namespace ProjectManager.Backend.DTOs.Projects
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string CustomerCompany { get; set; }
        public string ContractorCompany { get; set; }

        public int Priority { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ProjectEmployeeShortDto Manager { get; set; }

        public List<ProjectEmployeeShortDto> Employees { get; set; } =
            new List<ProjectEmployeeShortDto>();

        public List<ProjectFileShortDto> Files { get; set; } =
            new List<ProjectFileShortDto>();
    }
}
