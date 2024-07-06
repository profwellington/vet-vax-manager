using Dapper;
using VetVaxManager.Models;

namespace VetVaxManager.Repository;

public sealed class CalendarRepository : BaseRepository, ICalendarRepository
{
    public CalendarRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public IList<Calendar> GetAllEventsByAnimalId(int animalId)
    {
        using (var connection = CreateConnection())
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
        }
    }

    public int NewEvent(Calendar calendarEvent)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = @"
                                INSERT INTO agendas(data_hora, tempo_lembrete, id_animal, id_cartilha_vacinacao)
                                VALUES(@EventDateTime, @ReminderDays, @AnimalId, @VaccinationScheduleId)
                                RETURNING id;";

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
        }
    }

    public Calendar GetCalendarEventById(int id)
    {
        using (var connection = CreateConnection())
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
                        c.faixa_etaria AS AgeGroup,
                        e.id AS SpecieId,
                        e.nome AS Name
                    FROM agendas ag
                    INNER JOIN animais an ON an.id = ag.id_animal
                    INNER JOIN cartilhas_vacinacao c ON c.id = ag.id_cartilha_vacinacao
                    INNER JOIN especies e ON e.id = an.id_especie
                    WHERE ag.id = @CalendarId";

                var result = connection.Query<Calendar, Animal, VaccinationSchedule, Specie, Calendar>(
                    sql,
                    (calendar, animal, vaccinationSchedule, specie) =>
                    {
                        calendar.Animal = animal;
                        calendar.VaccinationSchedule = vaccinationSchedule;
                        calendar.Animal.Specie = specie;
                        return calendar;
                    },
                    new { CalendarId = id },
                    splitOn: "AnimalId, VaccinationScheduleId, SpecieId"
                ).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public int DeleteCalendarEventById(int id)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = "DELETE FROM agendas WHERE id = @Id";
                int count = connection.Execute(query, new { Id = id });
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public int UpdateCalendarEvent(Calendar calendarEvent)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = @"
                                UPDATE agendas
                                SET data_hora = @EventDateTime,
                                    tempo_lembrete = @ReminderDays,
                                    id_cartilha_vacinacao = @VaccinationScheduleId
                                WHERE id = @CalendarId";
                var parameters = new
                {
                    EventDateTime = calendarEvent.EventDateTime,
                    ReminderDays = calendarEvent.ReminderDays,
                    VaccinationScheduleId = calendarEvent.VaccinationSchedule.VaccinationScheduleId,
                    CalendarId = calendarEvent.CalendarId
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
