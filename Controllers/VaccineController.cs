using Microsoft.AspNetCore.Mvc;

namespace VetVaxManager.Controllers
{
    public class VaccineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
