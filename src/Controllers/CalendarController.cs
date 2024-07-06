using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VetVaxManager.Models;
using VetVaxManager.Repository;

namespace VetVaxManager.Controllers;

[Authorize]
public class CalendarController : Controller
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IVaccineRepository _vaccineRepository;
    private readonly ICalendarRepository _calendarRepository;

    public CalendarController(IAnimalRepository animalRepository, IVaccineRepository vaccineRepository, ICalendarRepository calendarRepository)
    {
        _animalRepository = animalRepository;
        _vaccineRepository = vaccineRepository;
        _calendarRepository = calendarRepository;
    }

    [HttpGet]
    public ActionResult NewEvent(int id)
    {
        var animal = _animalRepository.GetAnimalById(id);
        ViewBag.AnimalId = id;
        ViewBag.VaccinationSchedules = _vaccineRepository.GetVaccinationSchedules()
                                .Where(v => v.Specie.SpecieId == animal.Specie.SpecieId)
                                .ToList();
        return View();
    }

    [HttpPost]
    public ActionResult NewEvent(Calendar calendar)
    {
        calendar.Animal = _animalRepository.GetAnimalById(calendar.Animal.AnimalId);
        calendar.VaccinationSchedule = _vaccineRepository.GetVaccinationSchedules()
                                                    .FirstOrDefault(v => v.VaccinationScheduleId == calendar.VaccinationSchedule.VaccinationScheduleId);
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        if (ModelState.IsValid)
        {
            var calendarEventId = _calendarRepository.NewEvent(calendar);
            return RedirectToAction("Details", "Animal", new { id = calendar.Animal.AnimalId });
        }
        calendar.Animal = _animalRepository.GetAnimalById(calendar.Animal.AnimalId);
        return View(calendar);
    }

    [HttpGet]
    public IActionResult CalendarEventDetails(int id)
    {
        var calendarEvent = _calendarRepository.GetCalendarEventById(id);
        return View(calendarEvent);
    }

    [HttpGet]
    public IActionResult DeleteCalendarEvent(int id, int animalId)
    {
        var calendarEvent = _calendarRepository.DeleteCalendarEventById(id);
        var redirectId = animalId;
        return RedirectToAction("Details", "Animal", new { id = redirectId });
    }

    [HttpGet]
    public IActionResult EditEvent(int id)
    {
        var calendarEvent = _calendarRepository.GetCalendarEventById(id);
        ViewBag.VaccinationSchedules = _vaccineRepository.GetVaccinationSchedules()
                                .Where(v => v.Specie.SpecieId == calendarEvent.Animal.Specie.SpecieId)
                                .ToList();
        return View(calendarEvent);
    }

    [HttpPost]
    public ActionResult EditEvent(Calendar calendar)
    {
        calendar.Animal = _animalRepository.GetAnimalById(calendar.Animal.AnimalId);
        calendar.VaccinationSchedule = _vaccineRepository.GetVaccinationSchedules()
                                                    .FirstOrDefault(v => v.VaccinationScheduleId == calendar.VaccinationSchedule.VaccinationScheduleId);
        var errors = ModelState.Values.SelectMany(v => v.Errors);
        if (ModelState.IsValid)
        {
            var updatedCalendarEvent = _calendarRepository.UpdateCalendarEvent(calendar);
            return RedirectToAction("Details", "Animal", new { id = calendar.Animal.AnimalId });
        }
        calendar.Animal = _animalRepository.GetAnimalById(calendar.Animal.AnimalId);
        return View(calendar);
    }
}
