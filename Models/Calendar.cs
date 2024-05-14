namespace VetVaxManager.Models
{
    public class Calendar
    {
        public int CalendarId { get; set; }
        public DateTime EventDateTime { get; set; }
        public int ReminderDays { get; set; }
        public Animal Animal { get; set; }
        public VaccinationSchedule VaccinationSchedule { get; set; }
    }
}
