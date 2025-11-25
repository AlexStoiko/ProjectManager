using AutoMapper;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.Projects;

namespace ProjectManager.Backend.Mapping
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            // Project -> ProjectDto
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.Manager,
                    opt => opt.MapFrom(src => new ProjectEmployeeShortDto
                    {
                        Id = src.Manager.Id,
                        FullName = $"{src.Manager.LastName} {src.Manager.FirstName} {src.Manager.MiddleName}"
                    }))
                .ForMember(dest => dest.Employees,
                    opt => opt.MapFrom(src =>
                        src.ProjectEmployees.Select(pe => new ProjectEmployeeShortDto
                        {
                            Id = pe.Employee.Id,
                            FullName = $"{pe.Employee.LastName} {pe.Employee.FirstName} {pe.Employee.MiddleName}"
                        })))
                .ForMember(dest => dest.Files,
                    opt => opt.MapFrom(src =>
                        src.Files.Select(f => new ProjectFileShortDto
                        {
                            Id = f.Id,
                            FileName = f.FileName
                        })));

            // ProjectCreateDto -> Project
            CreateMap<ProjectCreateDto, Project>()
                .ForMember(dest => dest.ProjectEmployees, opt => opt.Ignore())
                .ForMember(dest => dest.Files, opt => opt.Ignore())
                .ForMember(dest => dest.Manager, opt => opt.Ignore());

            // ProjectUpdateDto -> Project
            CreateMap<ProjectUpdateDto, Project>()
                .ForMember(dest => dest.ProjectEmployees, opt => opt.Ignore())
                .ForMember(dest => dest.Files, opt => opt.Ignore())
                .ForMember(dest => dest.Manager, opt => opt.Ignore());
        }
    }
}
