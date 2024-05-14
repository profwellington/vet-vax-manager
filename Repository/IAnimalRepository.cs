using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface IAnimalRepository
    {
        Animal GetAnimalById(int id);
        IList<Animal> GetAnimalsByOwnerId(int ownerId);
    }
}
