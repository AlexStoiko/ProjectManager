using ProjectManager.Backend.DTOs.ProjectEmployees;

namespace ProjectManager.Backend.Services
{
    public interface IProjectEmployeeService
    {
        Task<IEnumerable<ProjectEmployeeDto>> GetAsync(int projectId);
        Task<ProjectEmployeeDto?> AddAsync(ProjectEmployeeCreateDto dto);
        Task<bool> RemoveAsync(int projectId, int employeeId);
    }
}
