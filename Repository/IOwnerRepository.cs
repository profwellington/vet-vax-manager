using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface IOwnerRepository
    {
        IList<Animal> GetAnimalsByOwnerId(int id);
    }
}
