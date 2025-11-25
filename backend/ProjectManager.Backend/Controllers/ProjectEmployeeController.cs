using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.ProjectEmployees;

namespace ProjectManager.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectEmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectEmployeeController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/projectemployee?projectId=1
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectEmployeeDto>>> Get(int projectId)
        {
            var list = await _context.ProjectEmployees
                .Where(pe => pe.ProjectId == projectId)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ProjectEmployeeDto>>(list));
        }

        // POST: api/projectemployee
        [HttpPost]
        public async Task<IActionResult> AddEmployee(ProjectEmployeeCreateDto dto)
        {
            // Проверяем что сотрудник существует
            var employeeExists = await _context.Employees.AnyAsync(e => e.Id == dto.EmployeeId);
            if (!employeeExists)
                return BadRequest("Указанного сотрудника не существует.");

            // Проверяем что проект существует
            var projectExists = await _context.Projects.AnyAsync(p => p.Id == dto.ProjectId);
            if (!projectExists)
                return BadRequest("Указанного проекта не существует.");

            // Проверяем, что эта связь не существует
            var exists = await _context.ProjectEmployees.AnyAsync(pe =>
                pe.EmployeeId == dto.EmployeeId &&
                pe.ProjectId == dto.ProjectId
            );

            if (exists)
                return BadRequest("Сотрудник уже добавлен в проект.");

            var entity = _mapper.Map<ProjectEmployee>(dto);

            _context.ProjectEmployees.Add(entity);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProjectEmployeeDto>(entity));
        }

        // DELETE: api/projectemployee?projectId=1&employeeId=2
        [HttpDelete]
        public async Task<IActionResult> RemoveEmployee(int projectId, int employeeId)
        {
            var entity = await _context.ProjectEmployees
                .FirstOrDefaultAsync(pe =>
                    pe.ProjectId == projectId &&
                    pe.EmployeeId == employeeId
                );

            if (entity == null)
                return NotFound("Такой связи проект–сотрудник нет.");

            _context.ProjectEmployees.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
