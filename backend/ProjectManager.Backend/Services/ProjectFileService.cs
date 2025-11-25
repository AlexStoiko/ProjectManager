using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.ProjectFiles;

namespace ProjectManager.Backend.Services
{
    public class ProjectFileService : IProjectFileService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public ProjectFileService(AppDbContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        // Returns all files for project
        public async Task<IEnumerable<ProjectFileDto>> GetFilesAsync(int projectId)
        {
            var files = await _context.ProjectFiles
                .Where(f => f.ProjectId == projectId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<ProjectFileDto>>(files);
        }

        // Uploads a file to project
        public async Task<ProjectFileDto?> UploadAsync(int projectId, ProjectFileUploadDto dto)
        {
            var project = await _context.Projects.FindAsync(projectId);
            if (project == null)
                return null;

            if (dto.File == null || dto.File.Length == 0)
                return null;

            var uploadsFolder = Path.Combine(_env.ContentRootPath, "uploads");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = File.Create(filePath))
            {
                await dto.File.CopyToAsync(stream);
            }

            var file = new ProjectFile
            {
                ProjectId = projectId,
                FileName = dto.File.FileName,
                FilePath = fileName,
                FileSize = dto.File.Length,
                UploadedAt = DateTime.UtcNow
            };

            _context.ProjectFiles.Add(file);
            await _context.SaveChangesAsync();

            return _mapper.Map<ProjectFileDto>(file);
        }

        // Deletes a project file by ID
        public async Task<bool> DeleteAsync(int projectId, int fileId)
        {
            var file = await _context.ProjectFiles
                .FirstOrDefaultAsync(f => f.Id == fileId && f.ProjectId == projectId);

            if (file == null)
                return false;

            var fullPath = Path.Combine(_env.ContentRootPath, "uploads", file.FilePath);

            if (File.Exists(fullPath))
                File.Delete(fullPath);

            _context.ProjectFiles.Remove(file);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
