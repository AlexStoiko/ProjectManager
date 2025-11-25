using AutoMapper;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.ProjectFiles;

namespace ProjectManager.Backend.Mapping
{
    public class ProjectFileProfile : Profile
    {
        public ProjectFileProfile()
        {
            CreateMap<ProjectFile, ProjectFileDto>();
        }
    }
}
