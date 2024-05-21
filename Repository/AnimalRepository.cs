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
        public Animal GetAnimalById(int id)
        {
            var connectionString = this.GetConnection();

            using (var connection = new MySqlConnection(connectionString))
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
                        a.id = @AnimalId";

                    var result = connection.Query<Animal, Specie, Owner, Animal>(
                        sql,
                        (animal, specie, owner) =>
                        {
                            animal.Specie = specie;
                            animal.Owner = owner;
                            return animal;
                        },
                        new { AnimalId = id },
                        splitOn: "SpecieId, OwnerId"
                    ).FirstOrDefault();

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
                            animal.Owner = owner;
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

        public int NewAnimal(Animal animal)
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = @"
                                INSERT INTO animais(nome, data_nascimento, sexo, raca, peso, vivo, id_proprietario, id_especie)
                                VALUES(@Name, @DateOfBirth, @Sex, @Race, @Weight, @Alive, @OwnerId, @SpecieId);
                                SELECT LAST_INSERT_ID();";

                    var parameters = new
                    {
                        Name = animal.Name,
                        DateOfBirth = animal.DateOfBirth,
                        Sex = animal.Sex,
                        Race = animal.Race,
                        Weight = animal.Weight,
                        Alive = animal.Alive,
                        OwnerId = animal.Owner.OwnerId,
                        SpecieId = animal.Specie.SpecieId
                    };

                    int id = connection.QuerySingle<int>(query, parameters);

                    return id;
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

        public IList<Specie> GetAllSpecies()
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = @"
                    SELECT
                        e.id AS SpecieId,
                        e.nome AS Name
                    FROM especies e";

                    var result = connection.Query<Specie>(
                        sql).ToList();

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
