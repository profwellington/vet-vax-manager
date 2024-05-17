using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface ICalendarRepository
    {
        IList<Calendar> GetAllEventsByAnimalId(int animalId);
    }
}
