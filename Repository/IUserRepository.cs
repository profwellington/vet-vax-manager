using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public interface IUserRepository
    {
        int NewUser(User user);
        User GetByEmail(string email);
    }
}
