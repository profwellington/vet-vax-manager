using Dapper;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public class AnimalRepository : IAnimalRepository
    {
        IConfiguration _configuration;
        public AnimalRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("MySQLConnection").Value;
            return connection;
        }
        public IList<Animal> GetAnimalsByOwnerId(int ownerId)
        {
            var connectionString = this.GetConnection();
            List<Animal> animals = new List<Animal>();

            using(var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = @"
                    SELECT 
                        a.id AS AnimalId,
                        a.nome AS Name,
                        a.data_nascimento AS DateOfBirth,
                        a.sexo AS Sex,
                        a.raca AS Race,
                        a.peso AS Weight,
                        a.vivo AS Alive,
                        s.id AS SpecieId,
                        s.nome AS Name,
                        p.id AS OwnerId,
                        p.nome AS Name,
                        p.sobrenome AS LastName,
                        p.data_nascimento AS DateOfBirth,
                        p.sexo AS Sex,
                        p.cpf AS Cpf,
                        p.email AS Email,
                        p.telefone AS Phone
                    FROM 
                        animais a
                    INNER JOIN 
                        especies s ON a.id_especie = s.id
                    INNER JOIN 
                        proprietarios p ON a.id_proprietario = p.id
                    WHERE 
                        a.id_proprietario = @OwnerId";

                    var result = connection.Query<Animal, Specie, Owner, Animal>(
                        sql,
                        (animal, specie, owner) =>
                        {
                            animal.Specie = specie;
                            animal.Owners = new List<Owner> { owner };
                            return animal;
                        },
                        new { OwnerId = ownerId },
                        splitOn: "SpecieId, OwnerId"
                    ).ToList();

                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
