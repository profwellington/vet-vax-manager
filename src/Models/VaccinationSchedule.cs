namespace VetVaxManager.Models;

public class VaccinationSchedule
{
    public int VaccinationScheduleId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Dose { get; set; }
    public string? AgeGroup { get; set; }
    public Specie? Specie { get; set; }
}
