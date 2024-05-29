using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection.PortableExecutable;
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
            if(vaccinationSchedule == null)
            {
                return NotFound();
            }
            return View(vaccinationSchedule);
        }

        public IActionResult VaccineDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Vaccine ID.");
            }

            var vaccine = _vaccineRepository.GetVaccineById(id);
            if (vaccine == null)
            {
                return NotFound();
            }
            return View(vaccine);
        }
        public IActionResult DeleteVaccine(int id, int animalId)
        {
            if (id <= 0 || animalId <= 0)
            {
                return BadRequest("Invalid Vaccine and/or Animal ID.");
            }

            var vaccine = _vaccineRepository.DeleteVaccineById(id);

            var redirectId = animalId;
            return RedirectToAction("Details", "Animal", new {id = redirectId});
        }

        public ActionResult NewVaccine(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Animal ID.");
            }

            var animal = _animalRepository.GetAnimalById(id);
            if (animal == null)
            {
                return NotFound();
            }

            ViewBag.AnimalId = id;
            ViewBag.VaccinationSchedules = _vaccineRepository.GetVaccinationSchedules()
                                    .Where(v => v.Specie.SpecieId == animal.Specie.SpecieId)
                                    .ToList();
            return View();
        }

        [HttpPost]
        public ActionResult NewVaccine(Vaccine vaccine)
        {
            if (vaccine == null)
            {
                return BadRequest("Vaccine data is required.");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var animal = _animalRepository.GetAnimalById(vaccine.Animal.AnimalId);
                if (animal == null)
                {
                    return NotFound("Animal not found.");
                }
                vaccine.Animal = animal;

                var vaccinationSchedule = vaccine.VaccinationSchedule = _vaccineRepository.GetVaccinationSchedules()
                                                            .FirstOrDefault(v => v.VaccinationScheduleId == vaccine.VaccinationSchedule.VaccinationScheduleId);
                if (vaccinationSchedule == null)
                {
                    ModelState.AddModelError("VaccinationSchedule", "Invalid vaccination schedule.");
                }

                var vaccineId = _vaccineRepository.NewVaccine(vaccine);
                return RedirectToAction("Details", "Animal", new { id = vaccine.Animal.AnimalId });
            }

            var animalRollback = _animalRepository.GetAnimalById(vaccine.Animal.AnimalId);
            if (animalRollback == null)
            {
                return NotFound("Animal not found.");
            }
            vaccine.Animal = animalRollback;
            return View(vaccine);
        }

        public IActionResult EditVaccine(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid Vaccine ID.");
            }

            var vaccine = _vaccineRepository.GetVaccineById(id);
            if (vaccine == null)
            {
                return NotFound();
            }

            var vaccinationSchedule = _vaccineRepository.GetVaccinationSchedules()
                                    .Where(v => v.Specie.SpecieId == vaccine.Animal.Specie.SpecieId)
                                    .ToList();
            if (vaccinationSchedule == null)
            {
                return NotFound();
            }

            ViewBag.VaccinationSchedules = vaccinationSchedule;
            return View(vaccine);
        }

        [HttpPost]
        public ActionResult EditVaccine(Vaccine vaccine)
        {
            var animal = _animalRepository.GetAnimalById(vaccine.Animal.AnimalId);
            if (animal == null)
            {
                return NotFound();
            }
            vaccine.Animal = animal;

            var vaccinationSchedule = _vaccineRepository.GetVaccinationSchedules()
                                                        .FirstOrDefault(v => v.VaccinationScheduleId == vaccine.VaccinationSchedule.VaccinationScheduleId);
            if (vaccinationSchedule == null)
            {
                return NotFound();
            }
            vaccine.VaccinationSchedule = vaccinationSchedule;

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {
                var updatedVaccine = _vaccineRepository.UpdateVaccine(vaccine);
                return RedirectToAction("Details", "Animal", new { id = vaccine.Animal.AnimalId });
            }

            vaccine.Animal = animal;

            return View(vaccine);
        }
    }
}
