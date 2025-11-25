using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.Projects;

namespace ProjectManager.Backend.Services
{
    public class ProjectService : IProjectService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Returns all projects with related data
        public async Task<IEnumerable<ProjectDto>> GetAllAsync() 
        {
            var projects = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.ProjectEmployees)
                    .ThenInclude(pe => pe.Employee)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectDto>>(projects);
        }

        // Returns a project by ID with full info
        public async Task<ProjectDto?> GetByIdAsync(int id) 
        {
            var project = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.ProjectEmployees)
                    .ThenInclude(pe => pe.Employee)
                .Include(p => p.Files)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return null;

            return _mapper.Map<ProjectDto>(project);
        }

        // Creates a new project
        public async Task<ProjectDto> CreateAsync(ProjectCreateDto dto) 
        {
            var project = _mapper.Map<Project>(dto);
            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        // Updates project by ID
        public async Task<ProjectDto?> UpdateAsync(int id, ProjectUpdateDto dto) 
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return null;

            _mapper.Map(dto, project);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectDto>(project);
        }

        // Deletes project by ID
        public async Task<bool> DeleteAsync(int id) 
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return false;

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();
            return true;
        }

        // Adds employees to project
        public async Task<bool> AddEmployeesAsync(int projectId, IEnumerable<int> employeeIds) 
        {
            var project = await _context.Projects
                .Include(p => p.ProjectEmployees)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null)
                return false;

            foreach (var id in employeeIds)
            {
                if (!project.ProjectEmployees.Any(pe => pe.EmployeeId == id))
                {
                    project.ProjectEmployees.Add(new ProjectEmployee
                    {
                        ProjectId = projectId,
                        EmployeeId = id
                    });
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // Removes employee from project
        public async Task<bool> RemoveEmployeeAsync(int projectId, int employeeId) 
        {
            var link = await _context.ProjectEmployees
                .FirstOrDefaultAsync(pe => pe.ProjectId == projectId && pe.EmployeeId == employeeId);

            if (link == null)
                return false;

            _context.ProjectEmployees.Remove(link);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
