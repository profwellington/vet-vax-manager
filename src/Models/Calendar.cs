using System.ComponentModel.DataAnnotations;

namespace VetVaxManager.Models;

public class Calendar
{
    public int CalendarId { get; set; }
    [Required]
    public DateTime EventDateTime { get; set; }
    [Required]
    public int ReminderDays { get; set; }
    [Required]
    public Animal? Animal { get; set; }
    [Required]
    public VaccinationSchedule? VaccinationSchedule { get; set; }
}
