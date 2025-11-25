using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.ProjectFiles;

namespace ProjectManager.Backend.Controllers
{
    [ApiController]
    [Route("api/projects/{projectId}/files")]
    public class ProjectFilesController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProjectFilesController(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        // GET: api/projects/{projectId}/files
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjectFileDto>>> GetFiles(int projectId)
        {
            var files = await _context.ProjectFiles
                .Where(f => f.ProjectId == projectId)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<ProjectFileDto>>(files));
        }

        // POST: api/projects/{projectId}/files (upload)
        [HttpPost]
        public async Task<ActionResult<ProjectFileDto>> UploadFile(
            int projectId,
            [FromForm] ProjectFileUploadDto dto)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                return NotFound("Проект не найден.");

            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("Файл пуст или не передан.");

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await dto.File.CopyToAsync(stream);
            }

            var projectFile = new ProjectFile
            {
                FileName = dto.File.FileName,
                FilePath = fileName,
                FileSize = dto.File.Length,
                ProjectId = projectId,
                UploadedAt = DateTime.UtcNow
            };

            _context.ProjectFiles.Add(projectFile);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<ProjectFileDto>(projectFile));
        }

        // DELETE: api/projects/{projectId}/files/{fileId}
        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(int projectId, int fileId)
        {
            var file = await _context.ProjectFiles
                .FirstOrDefaultAsync(f => f.Id == fileId && f.ProjectId == projectId);

            if (file == null)
                return NotFound("Файл не найден в данном проекте.");

            var fullPath = Path.Combine(_env.ContentRootPath, "uploads", file.FilePath);

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            _context.ProjectFiles.Remove(file);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
