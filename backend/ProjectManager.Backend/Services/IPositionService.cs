using ProjectManager.Backend.DTOs.Positions;

namespace ProjectManager.Backend.Services
{
    public interface IPositionService
    {
        Task<IEnumerable<PositionDto>> GetAllAsync();
        Task<PositionDto?> GetByIdAsync(int id);
        Task<PositionDto> CreateAsync(PositionCreateDto dto);
        Task<PositionDto?> UpdateAsync(int id, PositionUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
