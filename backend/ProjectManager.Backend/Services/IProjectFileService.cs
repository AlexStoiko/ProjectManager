using ProjectManager.Backend.DTOs.ProjectFiles;

namespace ProjectManager.Backend.Services
{
    public interface IProjectFileService
    {
        Task<IEnumerable<ProjectFileDto>> GetFilesAsync(int projectId);
        Task<ProjectFileDto?> UploadAsync(int projectId, ProjectFileUploadDto dto);
        Task<bool> DeleteAsync(int projectId, int fileId);
    }
}
