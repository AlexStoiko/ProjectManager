using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.Backend.Controllers.Mvc
{
    public class HomeController : Controller
    {
        // Returns main page
        public IActionResult Index()
        {
            return View();
        }
    }
}
