using ProjectManager.Backend.DTOs.Projects;

namespace ProjectManager.Backend.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<ProjectDto?> GetByIdAsync(int id);
        Task<ProjectDto> CreateAsync(ProjectCreateDto dto);
        Task<ProjectDto?> UpdateAsync(int id, ProjectUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<bool> AddEmployeesAsync(int projectId, IEnumerable<int> employeeIds);
        Task<bool> RemoveEmployeeAsync(int projectId, int employeeId);
    }
}
