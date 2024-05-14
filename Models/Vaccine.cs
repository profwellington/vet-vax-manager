namespace VetVaxManager.Models
{
    public class Vaccine
    {
        public int VaccineId { get; set; }
        public DateTime DateOfAdministration { get; set; }
        public string Lot { get; set; }
        public string Manufacturer { get; set; }
        public Animal Animal { get; set; }
        public VaccinationSchedule VaccinationSchedule { get; set; }
    }
}
