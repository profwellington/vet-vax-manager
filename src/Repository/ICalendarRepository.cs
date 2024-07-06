using VetVaxManager.Models;

namespace VetVaxManager.Repository;

public interface ICalendarRepository
{
    IList<Calendar> GetAllEventsByAnimalId(int animalId);
    int NewEvent(Calendar calendarEvent);
    Calendar GetCalendarEventById(int id);
    int DeleteCalendarEventById(int id);
    int UpdateCalendarEvent(Calendar calendarEvent);
}
