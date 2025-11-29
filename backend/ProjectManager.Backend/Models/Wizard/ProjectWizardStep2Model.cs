using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Backend.Models.Wizard
{
    public class ProjectWizardStep2Model
    {
        [Required(ErrorMessage = "Укажите компанию-заказчика")]
        [Display(Name = "Компания-заказчик")]
        public string CustomerCompany { get; set; }

        [Required(ErrorMessage = "Укажите компанию-исполнителя")]
        [Display(Name = "Компания-исполнитель")]
        public string ContractorCompany { get; set; }
    }
}
