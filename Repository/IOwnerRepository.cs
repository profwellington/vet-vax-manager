using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface IOwnerRepository
    {
        Owner GetOwnerById(int ownerId);
        int UpdateOwner(Owner owner);
    }
}
