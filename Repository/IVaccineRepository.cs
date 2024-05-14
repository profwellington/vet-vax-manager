using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface IVaccineRepository
    {
        IList<VaccinationSchedule> GetVaccinationSchedules();
        IList<Vaccine> GetAllVaccinesByAnimalId(int animalId);
    }
}
