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
    }
}
