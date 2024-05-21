using Dapper;
using MySql.Data.MySqlClient;
using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public class CalendarRepository : ICalendarRepository
    {
        IConfiguration _configuration;
        public CalendarRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("MySQLConnection").Value;
            return connection;
        }
        public IList<Calendar> GetAllEventsByAnimalId(int animalId)
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = @"
                    SELECT
                        ag.id AS CalendarId,
                        ag.data_hora AS EventDateTime,
                        ag.tempo_lembrete AS ReminderDays,
                        an.id AS AnimalId,
                        an.nome AS Name,
                        an.data_nascimento AS DateOfBirth,
                        an.sexo AS Sex,
                        an.raca AS Race,
                        an.peso AS Weight,
                        an.vivo AS Alive,
                        c.id AS VaccinationScheduleId,
                        c.nome_vacina AS Name,
                        c.descricao_vacina AS Description,
                        c.dose AS Dose,
                        c.faixa_etaria AS AgeGroup
                    FROM agendas ag
                    INNER JOIN animais an ON an.id = ag.id_animal
                    INNER JOIN cartilhas_vacinacao c ON c.id = ag.id_cartilha_vacinacao
                    WHERE ag.id_animal = @AnimalId";

                    var result = connection.Query<Calendar, Animal, VaccinationSchedule, Calendar>(
                        sql,
                        (calendar, animal, vaccinationSchedule) =>
                        {
                            calendar.Animal = animal;
                            calendar.VaccinationSchedule = vaccinationSchedule;
                            return calendar;
                        },
                        new { AnimalId = animalId },
                        splitOn: "AnimalId, VaccinationScheduleId"                        
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
        public int NewEvent(Calendar calendarEvent)
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var query = @"
                                INSERT INTO agendas(data_hora, tempo_lembrete, id_animal, id_cartilha_vacinacao)
                                VALUES(@EventDateTime, @ReminderDays, @AnimalId, @VaccinationScheduleId);
                                SELECT LAST_INSERT_ID();";

                    var parameters = new
                    {
                        EventDateTime = calendarEvent.EventDateTime,
                        ReminderDays = calendarEvent.ReminderDays,
                        AnimalId = calendarEvent.Animal.AnimalId,
                        VaccinationScheduleId = calendarEvent.VaccinationSchedule.VaccinationScheduleId
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
