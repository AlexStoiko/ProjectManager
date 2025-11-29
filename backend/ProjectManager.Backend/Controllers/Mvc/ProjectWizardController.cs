using Microsoft.AspNetCore.Mvc;
using ProjectManager.Backend.Models.Wizard;

namespace ProjectManager.Backend.Controllers.Mvc
{
    public class ProjectWizardController : Controller
    {
        // STEP 1 GET
        public IActionResult Step1()
        {
            var model = new ProjectWizardStep1Model();

            if (TempData.ContainsKey("ProjectName"))
                model.Name = TempData["ProjectName"] as string;

            if (TempData.ContainsKey("StartDate"))
            {
                model.StartDate = DateTime.Parse(TempData["StartDate"].ToString());
            }

            if (TempData.ContainsKey("EndDate"))
            {
                model.EndDate = DateTime.Parse(TempData["EndDate"].ToString());
            }

            if (TempData.ContainsKey("Priority"))
            {
                if (int.TryParse(TempData["Priority"] as string, out var pr))
                    model.Priority = pr;
            }

            if (!string.IsNullOrEmpty(model.Name)) TempData["ProjectName"] = model.Name;
            if (model.StartDate.HasValue) TempData["StartDate"] = model.StartDate.Value.ToString("yyyy-MM-dd");
            if (model.EndDate.HasValue) TempData["EndDate"] = model.EndDate.Value.ToString("yyyy-MM-dd");
            if (model.Priority.HasValue) TempData["Priority"] = model.Priority.Value.ToString();

            return View(model);
        }

        // STEP 1 POST
        [HttpPost]
        public IActionResult Step1(ProjectWizardStep1Model model)
        {
            if (!ModelState.IsValid)
                return View(model);

            TempData["ProjectName"] = model.Name;
            TempData["StartDate"] = model.StartDate?.ToString("yyyy-MM-dd");
            TempData["EndDate"] = model.EndDate?.ToString("yyyy-MM-dd");
            TempData["Priority"] = model.Priority?.ToString();

            return RedirectToAction("Step2");
        }

        // STEP 2 GET
        public IActionResult Step2()
        {
            var model = new ProjectWizardStep2Model();

            if (TempData.ContainsKey("CustomerCompany"))
                model.CustomerCompany = TempData["CustomerCompany"] as string;

            if (TempData.ContainsKey("ContractorCompany"))
                model.ContractorCompany = TempData["ContractorCompany"] as string;

            if (!string.IsNullOrEmpty(model.CustomerCompany)) TempData["CustomerCompany"] = model.CustomerCompany;
            if (!string.IsNullOrEmpty(model.ContractorCompany)) TempData["ContractorCompany"] = model.ContractorCompany;

            return View(model);
        }

        // STEP 2 POST
        [HttpPost]
        public IActionResult Step2(ProjectWizardStep2Model model)
        {
            if (!ModelState.IsValid)
                return View(model);

            TempData["CustomerCompany"] = model.CustomerCompany;
            TempData["ContractorCompany"] = model.ContractorCompany;

            return RedirectToAction("Step3");
        }

        // STEP 3 GET
        public IActionResult Step3()
        {
            var model = new ProjectWizardStep3Model();

            if (TempData.ContainsKey("ManagerId"))
                model.ManagerId = int.Parse(TempData["ManagerId"].ToString());

            if (TempData.ContainsKey("ManagerName"))
                model.ManagerName = TempData["ManagerName"].ToString();

            if (model.ManagerId.HasValue)
                TempData["ManagerId"] = model.ManagerId.Value;

            if (!string.IsNullOrEmpty(model.ManagerName))
                TempData["ManagerName"] = model.ManagerName;

            return View(model);
        }


        // STEP 3 POST
        [HttpPost]
        public IActionResult Step3(ProjectWizardStep3Model model)
        {
            if (!ModelState.IsValid)
                return View(model);

            TempData["ManagerId"] = model.ManagerId!.Value;
            TempData["ManagerName"] = model.ManagerName;

            return RedirectToAction("Step4");
        }

        // STEP 4 GET
        public IActionResult Step4()
        {
            var model = new ProjectWizardStep4Model();

            if (TempData.ContainsKey("SelectedEmployees"))
            {
                var ids = TempData["SelectedEmployees"] as string;
                model.SelectedEmployeeIds = ids.Split(',')
                    .Select(int.Parse)
                    .ToList();

                TempData["SelectedEmployees"] = ids;
            }

            return View(model);
        }

        // STEP 4 POST
        [HttpPost]
        public IActionResult Step4(ProjectWizardStep4Model model)
        {
            if (model.SelectedEmployeeIds == null || !model.SelectedEmployeeIds.Any())
            {
                ModelState.AddModelError("", "Выберите хотя бы одного исполнителя");
                return View(model);
            }

            TempData["SelectedEmployees"] = string.Join(",", model.SelectedEmployeeIds);

            return RedirectToAction("Step5");
        }

        // STEP 5 GET
        public IActionResult Step5()
        {
            return View(new ProjectWizardStep5Model());
        }

        // STEP 5 POST
        [HttpPost]
        public async Task<IActionResult> Step5(ProjectWizardStep5Model model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            foreach (var file in model.Files)
            {
                var filePath = Path.Combine(uploadPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }

            return RedirectToAction("Finish");
        }

    }
}
