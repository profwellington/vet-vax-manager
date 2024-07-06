using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface IAnimalRepository
    {
        Animal GetAnimalById(int id);
        IList<Animal> GetAnimalsByOwnerId(int ownerId);
        int NewAnimal(Animal animal);
        IList<Specie> GetAllSpecies();
        int DeleteAnimalById(int id);
        int UpdateAnimal(Animal animal);
    }
}
