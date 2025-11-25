using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.ProjectEmployees;

namespace ProjectManager.Backend.Services
{
    public class ProjectEmployeeService : IProjectEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectEmployeeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Returns all employees of project
        public async Task<IEnumerable<ProjectEmployeeDto>> GetAsync(int projectId)
        {
            var list = await _context.ProjectEmployees
                .Where(pe => pe.ProjectId == projectId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectEmployeeDto>>(list);
        }

        // Adds employee to project
        public async Task<ProjectEmployeeDto?> AddAsync(ProjectEmployeeCreateDto dto)
        {
            // Check employee exists
            var employeeExists = await _context.Employees.AnyAsync(e => e.Id == dto.EmployeeId);
            if (!employeeExists)
                return null;

            // Check project exists
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);
            if (!projectExists)
                return null;

            // Check link does not exist
            var exists = await _context.ProjectEmployees.AnyAsync(pe =>
                pe.EmployeeId == dto.EmployeeId &&
                pe.ProjectId == dto.ProjectId
            );
            if (exists)
                return null;

            var entity = _mapper.Map<ProjectEmployee>(dto);

            _context.ProjectEmployees.Add(entity);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectEmployeeDto>(entity);
        }

        // Removes employee from project
        public async Task<bool> RemoveAsync(int projectId, int employeeId)
        {
            var entity = await _context.ProjectEmployees
                .FirstOrDefaultAsync(pe =>
                    pe.ProjectId == projectId &&
                    pe.EmployeeId == employeeId
                );

            if (entity == null)
                return false;

            _context.ProjectEmployees.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
