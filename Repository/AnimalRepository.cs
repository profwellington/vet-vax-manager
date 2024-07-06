using Dapper;
using VetVaxManager.Models;

namespace VetVaxManager.Repository;

public sealed class AnimalRepository : BaseRepository, IAnimalRepository
{
    public AnimalRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public Animal GetAnimalById(int id)
    {
        using (var connection = CreateConnection())
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
        }
    }

    public IList<Animal> GetAnimalsByOwnerId(int ownerId)
    {
        using (var connection = CreateConnection())
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
        }
    }

    public int NewAnimal(Animal animal)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = @"
                                INSERT INTO animais(nome, data_nascimento, sexo, raca, peso, vivo, id_proprietario, id_especie)
                                VALUES(@Name, @DateOfBirth, @Sex, @Race, @Weight, @Alive, @OwnerId, @SpecieId)
                                RETURNING id;";

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
        }
    }

    public IList<Specie> GetAllSpecies()
    {
        using (var connection = CreateConnection())
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
        }
    }

    public int DeleteAnimalById(int id)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();

                var queryVaccine = "DELETE FROM vacinas WHERE id_animal = @Id";
                var querySchedule = "DELETE FROM agendas WHERE id_animal = @Id";
                var queryAnimals = "DELETE FROM animais WHERE id = @Id";

                connection.Execute(queryVaccine, new { Id = id });
                connection.Execute(querySchedule, new { Id = id });
                int countAnimals = connection.Execute(queryAnimals, new { Id = id });

                return countAnimals;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public int UpdateAnimal(Animal animal)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = @"
                                UPDATE animais
                                SET nome = @Name,
                                    data_nascimento = @DateOfBirth,
                                    sexo = @Sex,
                                    raca = @Race,
                                    peso = @Weight,
                                    vivo = @Alive,
                                    id_especie = @SpecieId
                                WHERE id = @AnimalId";

                var parameters = new
                {
                    Name = animal.Name,
                    DateOfBirth = animal.DateOfBirth,
                    Sex = animal.Sex,
                    Race = animal.Race,
                    Weight = animal.Weight,
                    Alive = animal.Alive,
                    SpecieId = animal.Specie.SpecieId,
                    AnimalId = animal.AnimalId
                };

                int count = connection.Execute(query, parameters);

                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
