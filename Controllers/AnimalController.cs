using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Reflection.PortableExecutable;
using VetVaxManager.Models;
using VetVaxManager.Repository;
using VetVaxManager.ViewModels;

namespace VetVaxManager.Controllers
{
    [Authorize]
    public class AnimalController : Controller
    {
        IAnimalRepository _animalRepository;
        IVaccineRepository _vaccineRepository;
        ICalendarRepository _calendarRepository;
        public AnimalController(IAnimalRepository animalRepository, IVaccineRepository vaccineRepository, ICalendarRepository calendarRepository)
        {
            _animalRepository = animalRepository;
            _vaccineRepository = vaccineRepository;
            _calendarRepository = calendarRepository;
        }
        public IActionResult Index()
        {            
            return View();
        }
        public IActionResult MyAnimals()
        {
            var ownerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "OwnerId");
            int ownerId = int.Parse(ownerIdClaim.Value);
            var animals = _animalRepository.GetAnimalsByOwnerId(ownerId);
            return View(animals);
        }
        public IActionResult Details(int id)
        {
            var animal = _animalRepository.GetAnimalById(id);
            var schedules = _calendarRepository.GetAllEventsByAnimalId(id);
            var vaccines = _vaccineRepository.GetAllVaccinesByAnimalId(id);

            AnimalDetailsViewModel animalDetails = new AnimalDetailsViewModel
            {
                Animal = animal,
                Calendar = schedules,
                Vaccines = vaccines
            };

            return View(animalDetails);
        }

        public ActionResult NewAnimal(int id)
        {
            var animal = _animalRepository.GetAnimalById(id);
            var ownerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "OwnerId");
            ViewBag.OwnerId = int.Parse(ownerIdClaim.Value);
            ViewBag.Species = _animalRepository.GetAllSpecies();
            return View();
        }

        [HttpPost]
        public ActionResult NewAnimal(Animal animal)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                animal.Specie = _animalRepository.GetAllSpecies().FirstOrDefault(s => s.SpecieId == animal.Specie.SpecieId);
                var animalId = _animalRepository.NewAnimal(animal);
                return RedirectToAction("MyAnimals", "Animal");
            }

            return View(animal);
        }

        public IActionResult DeleteAnimal(int id)
        {
            var animal = _animalRepository.DeleteAnimalById(id);
            return RedirectToAction("MyAnimals", "Animal");
        }

        public IActionResult EditAnimal(int id)
        {
            var animal = _animalRepository.GetAnimalById(id);
            ViewBag.Species = _animalRepository.GetAllSpecies();
            return View(animal);
        }

        [HttpPost]
        public ActionResult EditAnimal(Animal animal)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var updatedAnimal = _animalRepository.UpdateAnimal(animal);
                return RedirectToAction("MyAnimals", "Animal");
            }
            animal = _animalRepository.GetAnimalById(animal.AnimalId);

            return View(animal);
        }
    }
}
