using Microsoft.AspNetCore.Mvc;
using VetVaxManager.Repository;

namespace VetVaxManager.Controllers
{
    public class VaccineController : Controller
    {
        IVaccineRepository _vaccineRepository;
        public VaccineController(IVaccineRepository vaccineRepository)
        {
            _vaccineRepository = vaccineRepository;
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
    }
}
