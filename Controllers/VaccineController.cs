using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using VetVaxManager.Models;
using VetVaxManager.Repository;

namespace VetVaxManager.Controllers
{
    [Authorize]
    public class VaccineController : Controller
    {
        IAnimalRepository _animalRepository;
        IVaccineRepository _vaccineRepository;
        ICalendarRepository _calendarRepository;
        public VaccineController(IAnimalRepository animalRepository, IVaccineRepository vaccineRepository, ICalendarRepository calendarRepository)
        {
            _animalRepository = animalRepository;
            _vaccineRepository = vaccineRepository;
            _calendarRepository = calendarRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VaccinationSchedule()
        {
            var vaccinationSchedule = _vaccineRepository.GetVaccinationSchedules();
            return View(vaccinationSchedule);
        }

        public IActionResult VaccineRecord(int animalId)
        {
            var vaccines = _vaccineRepository.GetAllVaccinesByAnimalId(1);
            return View();
        }

        public IActionResult VaccineDetails(int id)
        {
            var vaccine = _vaccineRepository.GetVaccineById(id);
            return View(vaccine);
        }
        public IActionResult DeleteVaccine(int id, int animalId)
        {
            var vaccine = _vaccineRepository.DeleteVaccineById(id);
            var redirectId = animalId;
            return RedirectToAction("Details", "Animal", new {id = redirectId});
        }

        public ActionResult NewVaccine(int id)
        {
            var animal = _animalRepository.GetAnimalById(id);
            ViewBag.AnimalId = id;
            ViewBag.VaccinationSchedules = _vaccineRepository.GetVaccinationSchedules()
                                    .Where(v => v.Specie.SpecieId == animal.Specie.SpecieId)
                                    .ToList();
            return View();
        }

        [HttpPost]
        public ActionResult NewVaccine(Vaccine vaccine)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                vaccine.Animal = _animalRepository.GetAnimalById(vaccine.Animal.AnimalId);
                vaccine.VaccinationSchedule = _vaccineRepository.GetVaccinationSchedules()
                                                            .FirstOrDefault(v => v.VaccinationScheduleId == vaccine.VaccinationSchedule.VaccinationScheduleId);
                var vaccineId = _vaccineRepository.NewVaccine(vaccine);
                return RedirectToAction("Details", "Animal", new { id = vaccine.Animal.AnimalId });
            }

            vaccine.Animal = _animalRepository.GetAnimalById(vaccine.Animal.AnimalId);
            return View(vaccine);
        }

        public IActionResult EditVaccine(int id)
        {
            var vaccine = _vaccineRepository.GetVaccineById(id);
            ViewBag.VaccinationSchedules = _vaccineRepository.GetVaccinationSchedules()
                                    .Where(v => v.Specie.SpecieId == vaccine.Animal.Specie.SpecieId)
                                    .ToList();
            return View(vaccine);
        }

        [HttpPost]
        public ActionResult EditVaccine(Vaccine vaccine)
        {
            vaccine.Animal = _animalRepository.GetAnimalById(vaccine.Animal.AnimalId);
            vaccine.VaccinationSchedule = _vaccineRepository.GetVaccinationSchedules()
                                                        .FirstOrDefault(v => v.VaccinationScheduleId == vaccine.VaccinationSchedule.VaccinationScheduleId);
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var updatedVaccine = _vaccineRepository.UpdateVaccine(vaccine);
                return RedirectToAction("Details", "Animal", new { id = vaccine.Animal.AnimalId });
            }
            vaccine.Animal = _animalRepository.GetAnimalById(vaccine.Animal.AnimalId);

            return View(vaccine);
        }
    }
}
