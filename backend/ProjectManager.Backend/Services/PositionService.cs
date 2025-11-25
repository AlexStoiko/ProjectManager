using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.Positions;

namespace ProjectManager.Backend.Services
{
    public class PositionService : IPositionService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PositionService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Returns all positions
        public async Task<IEnumerable<PositionDto>> GetAllAsync()
        {
            var positions = await _context.Positions
                .Include(p => p.Employees)
                .ToListAsync();

            return _mapper.Map<IEnumerable<PositionDto>>(positions);
        }

        // Returns position by ID
        public async Task<PositionDto?> GetByIdAsync(int id)
        {
            var position = await _context.Positions
                .Include(p => p.Employees)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (position == null)
                return null;

            return _mapper.Map<PositionDto>(position);
        }

        // Creates new position
        public async Task<PositionDto> CreateAsync(PositionCreateDto dto)
        {
            var position = _mapper.Map<Position>(dto);
            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            return _mapper.Map<PositionDto>(position);
        }

        // Updates position by ID
        public async Task<PositionDto?> UpdateAsync(int id, PositionUpdateDto dto)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
                return null;

            _mapper.Map(dto, position);
            await _context.SaveChangesAsync();

            return _mapper.Map<PositionDto>(position);
        }

        // Deletes position by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
                return false;

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
