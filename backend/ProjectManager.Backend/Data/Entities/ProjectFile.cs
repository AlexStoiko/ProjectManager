using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectManager.Backend.Data.Entities
{
    [Table("ProjectFile")] 
    public class ProjectFile
    {
        public int Id { get; set; }

        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
