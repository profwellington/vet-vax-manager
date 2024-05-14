using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface IAnimalRepository
    {
        IList<Animal> GetAnimalsByOwnerId(int ownerId);
    }
}
