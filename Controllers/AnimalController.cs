using Microsoft.AspNetCore.Mvc;

namespace VetVaxManager.Controllers
{
    public class AnimalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
