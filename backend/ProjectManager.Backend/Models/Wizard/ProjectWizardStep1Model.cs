using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Backend.Models.Wizard
{
    public class ProjectWizardStep1Model
    {
        [Required(ErrorMessage = "Введите название проекта")]
        [Display(Name = "Название проекта")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Укажите дату начала")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime? StartDate { get; set; }

        [Required(ErrorMessage = "Укажите дату окончания")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime? EndDate { get; set; }

        [Required(ErrorMessage = "Укажите приоритет проекта")]
        [Range(1, 10, ErrorMessage = "Приоритет должен быть от {1} до {2}")]
        [Display(Name = "Приоритет")]
        public int? Priority { get; set; }
    }
}
