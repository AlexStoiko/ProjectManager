using Microsoft.AspNetCore.Mvc;
using ProjectManager.Backend.Services;
using ProjectManager.Backend.Models;
using ProjectManager.Backend.DTOs.Projects;
using System.Globalization;

namespace ProjectManager.Backend.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IEmployeeService _employeeService;

        public ProjectsController(IProjectService projectService, IEmployeeService employeeService)
        {
            _projectService = projectService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> Index(
            DateTime? startFrom,
            DateTime? startTo,
            int? minPriority,
            int? maxPriority,
            string? search,
            string sortBy = "StartDate",
            string sortDir = "asc")
        {
            var projects = (await _projectService.GetAllAsync()).ToList();

            if (startFrom.HasValue)
                projects = projects.Where(p => p.StartDate.Date >= startFrom.Value.Date).ToList();
            if (startTo.HasValue)
                projects = projects.Where(p => p.StartDate.Date <= startTo.Value.Date).ToList();

            if (minPriority.HasValue)
                projects = projects.Where(p => p.Priority >= minPriority.Value).ToList();
            if (maxPriority.HasValue)
                projects = projects.Where(p => p.Priority <= maxPriority.Value).ToList();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var q = search.Trim().ToLower(CultureInfo.CurrentCulture);
                projects = projects.Where(p =>
                    (!string.IsNullOrEmpty(p.Title) && p.Title.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(p.CustomerCompany) && p.CustomerCompany.ToLower().Contains(q)) ||
                    (!string.IsNullOrEmpty(p.ContractorCompany) && p.ContractorCompany.ToLower().Contains(q))
                ).ToList();
            }

            sortBy = (sortBy ?? "StartDate").Trim();
            sortDir = (sortDir ?? "asc").ToLower();

            Func<ProjectDto, object?> keySelector = sortBy switch
            {
                "Title" => new Func<ProjectDto, object?>(p => p.Title),
                "Customer" => p => p.CustomerCompany,
                "Contractor" => p => p.ContractorCompany,
                "Priority" => p => p.Priority,
                "StartDate" => p => p.StartDate,
                "EndDate" => p => p.EndDate,
                _ => p => p.StartDate
            };

            projects = (sortDir == "desc")
                ? projects.OrderByDescending(keySelector).ToList()
                : projects.OrderBy(keySelector).ToList();

            ViewData["startFrom"] = startFrom?.ToString("yyyy-MM-dd");
            ViewData["startTo"] = startTo?.ToString("yyyy-MM-dd");
            ViewData["minPriority"] = minPriority?.ToString();
            ViewData["maxPriority"] = maxPriority?.ToString();
            ViewData["search"] = search ?? "";
            ViewData["sortBy"] = sortBy;
            ViewData["sortDir"] = sortDir;

            return View(projects);
        }

        public async Task<IActionResult> Details(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null)
                return NotFound();

            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetByIdAsync(id);
            if (project == null) return NotFound();

            var model = new ProjectUpdateViewModel
            {
                Id = project.Id,
                Title = project.Title,
                CustomerCompany = project.CustomerCompany,
                ContractorCompany = project.ContractorCompany,
                Priority = project.Priority,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                ManagerId = project.Manager?.Id,
                EmployeeIds = project.Employees?.Select(e => e.Id).ToList() ?? new()
            };

            var employees = await _employeeService.GetAllAsync();
            ViewBag.Employees = employees;

            return View(model);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProjectUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Employees = await _employeeService.GetAllAsync();
                return View(model);
            }

            var dto = new DTOs.Projects.ProjectUpdateDto
            {
                Title = model.Title,
                CustomerCompany = model.CustomerCompany,
                ContractorCompany = model.ContractorCompany,
                Priority = model.Priority,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                ManagerId = model.ManagerId ?? 0,
                EmployeeIds = model.EmployeeIds ?? new()
            };

            var updated = await _projectService.UpdateAsync(model.Id, dto);
            if (updated == null) return NotFound();

            if (model.EmployeeIds != null && model.EmployeeIds.Any())
            {
                await _projectService.AddEmployeesAsync(model.Id, model.EmployeeIds);
            }

            return RedirectToAction(nameof(Details), new { id = model.Id });
        }

        // POST: Projects/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _projectService.DeleteAsync(id);
            if (!ok) return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
