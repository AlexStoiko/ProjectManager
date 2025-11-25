using AutoMapper;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.Data.DTOs.Employee;

namespace ProjectManager.Backend.Mapping
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Employee → EmployeeDto
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.PositionName,
                    opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : null));

            // CreateEmployeeDto → Employee
            CreateMap<CreateEmployeeDto, Employee>();

            // UpdateEmployeeDto → Employee
            CreateMap<UpdateEmployeeDto, Employee>();
        }
    }
}
