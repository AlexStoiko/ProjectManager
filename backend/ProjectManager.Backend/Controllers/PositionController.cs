using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.DTOs.Positions;
using AutoMapper;

namespace ProjectManager.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PositionController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/position
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PositionDto>>> GetAll()
        {
            var positions = await _context.Positions.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<PositionDto>>(positions));
        }

        // GET: api/position/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PositionDto>> GetById(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
                return NotFound();

            return Ok(_mapper.Map<PositionDto>(position));
        }

        // POST: api/position
        [HttpPost]
        public async Task<ActionResult<PositionDto>> Create(PositionCreateDto dto)
        {
            var position = _mapper.Map<Position>(dto);

            _context.Positions.Add(position);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetById),
                new { id = position.Id },
                _mapper.Map<PositionDto>(position)
            );
        }

        // PUT: api/position/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, PositionUpdateDto dto)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
                return NotFound();

            _mapper.Map(dto, position);

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/position/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var position = await _context.Positions.FindAsync(id);
            if (position == null)
                return NotFound();

            _context.Positions.Remove(position);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
