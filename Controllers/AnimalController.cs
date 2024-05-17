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
        public AnimalController(IAnimalRepository animalRepository, IVaccineRepository vaccineRepository)
        {
            _animalRepository = animalRepository;
            _vaccineRepository = vaccineRepository;
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
        public IActionResult Details(int animalId)
        {
            var animal = _animalRepository.GetAnimalById(1);
            // TODO: Metodo para trazer informacoes de Agendas
            var vaccines = _vaccineRepository.GetAllVaccinesByAnimalId(1);

            AnimalDetailsViewModel animalDetails = null;

            return View(animalDetails);
        }
    }
}
