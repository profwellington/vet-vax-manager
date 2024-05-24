using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface IUserRepository
    {
        int NewUser(User user);
        User GetByUsername(string username);
    }
}
