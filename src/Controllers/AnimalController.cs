using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetVaxManager.Models;
using VetVaxManager.Repository;
using VetVaxManager.ViewModels;

namespace VetVaxManager.Controllers;

[Authorize]
public class AnimalController : Controller
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IVaccineRepository _vaccineRepository;
    private readonly ICalendarRepository _calendarRepository;
    
    public AnimalController(IAnimalRepository animalRepository, IVaccineRepository vaccineRepository, ICalendarRepository calendarRepository)
    {
        _animalRepository = animalRepository;
        _vaccineRepository = vaccineRepository;
        _calendarRepository = calendarRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {            
        return View();
    }
    
    [HttpGet]
    public IActionResult MyAnimals()
    {
        var ownerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "OwnerId");
        int ownerId = int.Parse(ownerIdClaim.Value);
        var animals = _animalRepository.GetAnimalsByOwnerId(ownerId);
        return View(animals);
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        Animal? animal = _animalRepository.GetAnimalById(id);
        var schedules = _calendarRepository.GetAllEventsByAnimalId(id);
        var vaccines = _vaccineRepository.GetAllVaccinesByAnimalId(id);

        AnimalDetailsViewModel animalDetails = new()
        {
            Animal = animal,
            Calendar = schedules,
            Vaccines = vaccines
        };
        return View(animalDetails);
    }

    [HttpGet]
    public ActionResult NewAnimal(int id)
    {
        Animal? animal = _animalRepository.GetAnimalById(id);
        var ownerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "OwnerId");
        ViewBag.OwnerId = int.Parse(ownerIdClaim.Value);
        ViewBag.Species = _animalRepository.GetAllSpecies();
        return View();
    }

    [HttpPost]
    public ActionResult NewAnimal(Animal animal)
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        if (!ModelState.IsValid)
            return View(animal);

        animal.Specie = _animalRepository.GetAllSpecies().FirstOrDefault(s => s.SpecieId == animal.Specie?.SpecieId);
        int animalId = _animalRepository.NewAnimal(animal);
        return RedirectToAction("MyAnimals", "Animal");
    }

    [HttpGet]
    public IActionResult DeleteAnimal(int id)
    {
        _ = _animalRepository.DeleteAnimalById(id);
        return RedirectToAction("MyAnimals", "Animal");
    }

    [HttpGet]
    public IActionResult EditAnimal(int id)
    {
        Animal? animal = _animalRepository.GetAnimalById(id);
        ViewBag.Species = _animalRepository.GetAllSpecies();
        return View(animal);
    }

    [HttpPost]
    public ActionResult EditAnimal(Animal animal)
    {
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        if (ModelState.IsValid)
        {
            int updatedAnimal = _animalRepository.UpdateAnimal(animal);
            return RedirectToAction("MyAnimals", "Animal");
        }
        animal = _animalRepository.GetAnimalById(animal.AnimalId);
        return View(animal);
    }
}
