using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Backend.Models.Wizard
{
    public class ProjectWizardStep3Model
    {
        [Required(ErrorMessage = "Выберите руководителя проекта")]
        [Display(Name = "Руководитель проекта")]
        public int? ManagerId { get; set; }

        public string? ManagerName { get; set; }
    }
}
