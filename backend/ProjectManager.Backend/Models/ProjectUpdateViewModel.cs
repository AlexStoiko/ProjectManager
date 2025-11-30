using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Backend.Models
{
    public class ProjectUpdateViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название проекта")]
        public string Title { get; set; }

        [Display(Name = "Компания - заказчик")]
        public string CustomerCompany { get; set; }

        [Display(Name = "Компания - исполнитель")]
        public string ContractorCompany { get; set; }

        [Range(1, 10)]
        [Display(Name = "Приоритет")]
        public int Priority { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата начала")]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Дата окончания")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Руководитель")]
        public int? ManagerId { get; set; }

        [Display(Name = "Исполнители")]
        public List<int> EmployeeIds { get; set; } = new();
    }
}
