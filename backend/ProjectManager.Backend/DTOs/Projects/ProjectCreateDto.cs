using System;
using System.Collections.Generic;

namespace ProjectManager.Backend.DTOs.Projects
{
    public class ProjectCreateDto
    {
        public string Title { get; set; }
        public string CustomerCompany { get; set; }
        public string ContractorCompany { get; set; }

        public int Priority { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ManagerId { get; set; }

        public List<int> EmployeeIds { get; set; } =
            new List<int>();
    }
}
