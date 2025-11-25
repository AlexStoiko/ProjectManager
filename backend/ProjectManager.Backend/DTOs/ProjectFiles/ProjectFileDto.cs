namespace ProjectManager.Backend.DTOs.ProjectFiles
{
    public class ProjectFileDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }

        public int ProjectId { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
