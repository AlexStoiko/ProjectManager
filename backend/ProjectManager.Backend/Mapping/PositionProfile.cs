using AutoMapper;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.Positions;

namespace ProjectManager.Backend.Mapping
{
    public class PositionProfile : Profile
    {
        public PositionProfile()
        {
            CreateMap<Position, PositionDto>();
            CreateMap<PositionCreateDto, Position>();
            CreateMap<PositionUpdateDto, Position>();
        }
    }
}
