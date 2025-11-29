using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ProjectManager.Backend.Models.Wizard
{
    public class ProjectWizardStep5Model
    {
        [Required(ErrorMessage = "Выберите хотя бы один файл.")]
        public List<IFormFile> Files { get; set; } = new();
    }
}
