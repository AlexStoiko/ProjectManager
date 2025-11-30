using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectManager.Backend.Models
{
    public class EmployeeFormViewModel
    {
        public int? Id { get; set; }

        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public string Email { get; set; } = "";
        public int? PositionId { get; set; }
        public string? NewPositionName { get; set; }
        public IEnumerable<SelectListItem> Positions { get; set; } = new List<SelectListItem>();
    }
}
