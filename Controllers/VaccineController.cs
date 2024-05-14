﻿using Microsoft.AspNetCore.Mvc;
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
            return View();
        }
    }
}
