using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectManager.Backend.Models;
using ProjectManager.Backend.Data.DTOs.Employee;
using ProjectManager.Backend.DTOs.Positions;
using ProjectManager.Backend.Services;

namespace ProjectManager.Backend.Controllers.Mvc
{
    public class EmployeesMvcController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;

        public EmployeesMvcController(IEmployeeService employeeService, IPositionService positionService)
        {
            _employeeService = employeeService;
            _positionService = positionService;
        }

        // INDEX
        public async Task<IActionResult> Index()
        {
            var list = await _employeeService.GetAllAsync();
            return View(list);
        }

        // DETAILS
        public async Task<IActionResult> Details(int id)
        {
            var emp = await _employeeService.GetByIdAsync(id);
            if (emp == null) return NotFound();
            return View(emp);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            var vm = new EmployeeFormViewModel();
            await FillPositions(vm);
            return View(vm);
        }

        // POST: Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await FillPositions(vm);
                return View(vm);
            }

            int? positionId = vm.PositionId;
            if (!string.IsNullOrWhiteSpace(vm.NewPositionName))
            {
                var posDto = new PositionCreateDto { Name = vm.NewPositionName.Trim() };
                var created = await _positionService.CreateAsync(posDto);
                positionId = created?.Id;
            }

            var createDto = new CreateEmployeeDto
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                MiddleName = vm.MiddleName,
                Email = vm.Email,
                PositionId = positionId
            };

            await _employeeService.CreateAsync(createDto);

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var e = await _employeeService.GetByIdAsync(id);
            if (e == null) return NotFound();

            var vm = new EmployeeFormViewModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Email = e.Email,
                PositionId = e.PositionId
            };

            await FillPositions(vm);
            return View(vm);
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EmployeeFormViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await FillPositions(vm);
                return View(vm);
            }

            int? positionId = vm.PositionId;
            if (!string.IsNullOrWhiteSpace(vm.NewPositionName))
            {
                var posDto = new PositionCreateDto { Name = vm.NewPositionName.Trim() };
                var created = await _positionService.CreateAsync(posDto);
                positionId = created?.Id;
            }

            var updateDto = new UpdateEmployeeDto
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                MiddleName = vm.MiddleName,
                Email = vm.Email,
                PositionId = positionId
            };

            var updated = await _employeeService.UpdateAsync(id, updateDto);
            if (updated == null) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // POST: Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _positionService.DeleteAsync(id);
            if (!ok) return NotFound();
            return RedirectToAction(nameof(Index));
        }

        private async Task FillPositions(EmployeeFormViewModel vm)
        {
            var positions = await _positionService.GetAllAsync();
            vm.Positions = positions.Select(p =>
                new SelectListItem(p.Name, p.Id.ToString(), vm.PositionId.HasValue && vm.PositionId.Value == p.Id)
            ).ToList();
        }
    }
}
