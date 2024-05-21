using Dapper;
using MySql.Data.MySqlClient;
using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public class VaccineRepository : IVaccineRepository
    {
        IConfiguration _configuration;
        public VaccineRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("MySQLConnection").Value;
            return connection;
        }
        public IList<VaccinationSchedule> GetVaccinationSchedules()
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = @"
                    SELECT
                        c.id AS VaccinationScheduleId,
                        c.nome_vacina AS Name,
                        c.descricao_vacina AS Description,
                        c.dose AS Dose,
                        c.faixa_etaria AS AgeGroup,
                        e.id AS SpecieId,
                        e.nome AS Name
                    FROM cartilhas_vacinacao c
                    INNER JOIN especies e ON e.id = c.id_especie";

                    var result = connection.Query<VaccinationSchedule, Specie, VaccinationSchedule>(
                        sql,
                        (vaccinationSchedule, specie) =>
                        {
                            vaccinationSchedule.Specie = specie;
                            return vaccinationSchedule;
                        },
                        splitOn: "SpecieId"
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
        public IList<Vaccine> GetAllVaccinesByAnimalId(int animalId)
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = @"
                    SELECT
                        v.id AS VaccineId,
                        v.data_administracao AS DateOfAdministration,
                        v.lote AS Lot,
                        v.fabricante AS Manufacturer,
                        v.data_fabricacao AS DateOfManufacture,
                        c.id AS VaccinationScheduleId,
                        c.nome_vacina AS Name,
                        c.descricao_vacina AS Description,
                        c.dose AS Dose,
                        c.faixa_etaria AS AgeGroup,
                        a.id AS AnimalId,
                        a.nome AS Name,
                        a.data_nascimento AS DateOfBirth,
                        a.sexo AS Sex,
                        a.raca AS Race,
                        a.peso AS Weight,
                        a.vivo AS Alive
                    FROM vacinas v
                    INNER JOIN cartilhas_vacinacao c ON c.id = v.id_cartilha_vacinacao
                    INNER JOIN animais a ON a.id = v.id_animal
                    WHERE v.id_animal = @AnimalId";

                    var result = connection.Query<Vaccine, VaccinationSchedule, Animal, Vaccine>(
                        sql,
                        (vaccine, vaccinationSchedule, animal) =>
                        {
                            vaccine.Animal = animal;
                            vaccine.VaccinationSchedule = vaccinationSchedule;
                            return vaccine;
                        },
                        new { AnimalId = animalId },
                        splitOn: "VaccinationScheduleId, AnimalId"
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
        public Vaccine GetVaccineById(int id)
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = @"
                    SELECT
                        v.id AS VaccineId,
                        v.data_administracao AS DateOfAdministration,
                        v.lote AS Lot,
                        v.fabricante AS Manufacturer,
                        v.data_fabricacao AS DateOfManufacture,
                        c.id AS VaccinationScheduleId,
                        c.nome_vacina AS Name,
                        c.descricao_vacina AS Description,
                        c.dose AS Dose,
                        c.faixa_etaria AS AgeGroup,
                        a.id AS AnimalId,
                        a.nome AS Name,
                        a.data_nascimento AS DateOfBirth,
                        a.sexo AS Sex,
                        a.raca AS Race,
                        a.peso AS Weight,
                        a.vivo AS Alive
                    FROM vacinas v
                    INNER JOIN cartilhas_vacinacao c ON c.id = v.id_cartilha_vacinacao
                    INNER JOIN animais a ON a.id = v.id_animal
                    WHERE v.id = @VaccineId";

                    var result = connection.Query<Vaccine, VaccinationSchedule, Animal, Vaccine>(
                        sql,
                        (vaccine, vaccinationSchedule, animal) =>
                        {
                            vaccine.Animal = animal;
                            vaccine.VaccinationSchedule = vaccinationSchedule;
                            return vaccine;
                        },
                        new { VaccineId = id },
                        splitOn: "VaccinationScheduleId, AnimalId"
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
        public int DeleteVaccineById(int id)
        {
            var connectionString = this.GetConnection();
            var count = 0;
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = "DELETE FROM vacinas WHERE id =" + id;
                    count = connection.Execute(query);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
                return count;
            }
        }

        public int NewVaccine(Vaccine vaccine)
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = @"
                                INSERT INTO vacinas(data_administracao, lote, fabricante, data_fabricacao, id_animal, id_cartilha_vacinacao)
                                VALUES(@DataAdministracao, @Lote, @Fabricante, @DataFabricacao, @AnimalId, @VaccinationScheduleId);
                                SELECT LAST_INSERT_ID();";

                    var parameters = new
                    {
                        DataAdministracao = vaccine.DateOfAdministration,
                        Lote = vaccine.Lot,
                        Fabricante = vaccine.Manufacturer,
                        DataFabricacao = vaccine.DateOfManufacture,
                        AnimalId = vaccine.Animal.AnimalId,
                        VaccinationScheduleId = vaccine.VaccinationSchedule.VaccinationScheduleId
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
    }
}
