using Microsoft.AspNetCore.Mvc;
using VetVaxManager.Models;
using VetVaxManager.Repository;
using VetVaxManager.ViewModels;

namespace VetVaxManager.Controllers
{
    public class CalendarController : Controller
    {
        IAnimalRepository _animalRepository;
        IVaccineRepository _vaccineRepository;
        ICalendarRepository _calendarRepository;
        public CalendarController(IAnimalRepository animalRepository, IVaccineRepository vaccineRepository, ICalendarRepository calendarRepository)
        {
            _animalRepository = animalRepository;
            _vaccineRepository = vaccineRepository;
            _calendarRepository = calendarRepository;
        }

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
    }
}
