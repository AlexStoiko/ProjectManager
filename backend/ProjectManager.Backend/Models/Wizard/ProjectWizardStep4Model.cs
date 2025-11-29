using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Backend.Models.Wizard
{
    public class ProjectWizardStep4Model
    {
        [Required(ErrorMessage = "Выберите хотя бы одного исполнителя")]
        public List<int> SelectedEmployeeIds { get; set; } = new();

        public List<EmployeeShortViewModel> SelectedEmployees { get; set; } = new();
    }

    public class EmployeeShortViewModel
    {
        public int Id { get; set; }
        public string FullName { get; set; } = "";
    }
}
