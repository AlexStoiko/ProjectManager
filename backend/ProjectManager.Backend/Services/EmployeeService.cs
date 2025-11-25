using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Backend.Data;
using ProjectManager.Backend.Data.Entities;
using ProjectManager.Backend.Data.DTOs.Employee;

namespace ProjectManager.Backend.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // Returns all employees with position
        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var employees = await _context.Employees
                .Include(e => e.Position)
                .ToListAsync();

            return _mapper.Map<IEnumerable<EmployeeDto>>(employees);
        }

        // Returns employee by ID with position
        public async Task<EmployeeDto?> GetByIdAsync(int id)
        {
            var employee = await _context.Employees
                .Include(e => e.Position)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
                return null;

            return _mapper.Map<EmployeeDto>(employee);
        }

        // Creates new employee
        public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto)
        {
            var employee = _mapper.Map<Employee>(dto);
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        // Updates employee by ID
        public async Task<EmployeeDto?> UpdateAsync(int id, UpdateEmployeeDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return null;

            _mapper.Map(dto, employee);
            await _context.SaveChangesAsync();

            return _mapper.Map<EmployeeDto>(employee);
        }

        // Deletes employee by ID
        public async Task<bool> DeleteAsync(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
                return false;

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
