using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using VetVaxManager.Repository;

namespace VetVaxManager.Controllers
{
    public class AnimalController : Controller
    {
        IAnimalRepository _animalRepository;
        public AnimalController(IAnimalRepository animalRepository)
        {
            _animalRepository = animalRepository;
        }
        public IActionResult Index()
        {
            var animals = _animalRepository.GetAnimalsByOwnerId(1);
            return View();
        }
        public IActionResult Detail(int animalId)
        {
            var animals = _animalRepository.GetAnimalById(1);
            return View();
        }
    }
}
