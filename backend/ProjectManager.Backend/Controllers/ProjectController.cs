using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.Projects;

namespace ProjectManager.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ProjectController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/project
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectDto>>> GetProjects()
        {
            var projects = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.ProjectEmployees).ThenInclude(pe => pe.Employee)
                .Include(p => p.Files)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ProjectDto>>(projects));
        }

        // GET: api/project/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ProjectDto>> GetProject(int id)
        {
            var project = await _context.Projects
                .Include(p => p.Manager)
                .Include(p => p.ProjectEmployees).ThenInclude(pe => pe.Employee)
                .Include(p => p.Files)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (project == null)
                return NotFound();

            return Ok(_mapper.Map<ProjectDto>(project));
        }

        // POST: api/project
        [HttpPost]
        public async Task<ActionResult<ProjectDto>> CreateProject(ProjectCreateDto dto)
        {
            var project = _mapper.Map<Project>(dto);

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.Id },
                _mapper.Map<ProjectDto>(project));
        }

        // PUT: api/project/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProject(int id, ProjectUpdateDto dto)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            _mapper.Map(dto, project);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/project/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
                return NotFound();

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
