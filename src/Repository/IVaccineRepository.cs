using VetVaxManager.Models;

namespace VetVaxManager.Repository;

public interface IVaccineRepository
{
    IList<VaccinationSchedule> GetVaccinationSchedules();
    IList<Vaccine> GetAllVaccinesByAnimalId(int animalId);
    Vaccine GetVaccineById(int id);
    int DeleteVaccineById(int id);
    int NewVaccine(Vaccine vaccine);
    int UpdateVaccine(Vaccine vaccine);
}
