using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Configuration;
using VetVaxManager.Models;
using VetVaxManager.Repository;
using VetVaxManager.ViewModels;

namespace VetVaxManager.Controllers
{
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
            var animals = _animalRepository.GetAnimalsByOwnerId(1);
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
    }
}
