using AutoMapper;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.ProjectEmployees;

namespace ProjectManager.Backend.Mapping
{
    public class ProjectEmployeeProfile : Profile
    {
        public ProjectEmployeeProfile()
        {
            CreateMap<ProjectEmployee, ProjectEmployeeDto>();
            CreateMap<ProjectEmployeeCreateDto, ProjectEmployee>();
        }
    }
}
