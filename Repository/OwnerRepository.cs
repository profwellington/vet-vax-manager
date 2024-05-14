using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public class OwnerRepository : IOwnerRepository
    {
        IConfiguration _configuration;
        public OwnerRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("MySQLConnection").Value;
            return connection;
        }
        public IList<Animal> GetAnimalsByOwnerId(int id)
        {
            var connectionString = this.GetConnection();
            List<Animal> animals = new List<Animal>();
            return null;
        }
    }
}
