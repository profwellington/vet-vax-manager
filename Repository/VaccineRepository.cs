using Dapper;
using VetVaxManager.Models;

namespace VetVaxManager.Repository;

public class VaccineRepository : BaseRepository, IVaccineRepository
{
    public VaccineRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public IList<VaccinationSchedule> GetVaccinationSchedules()
    {
        using (var connection = CreateConnection())
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
                            e.nome AS SpecieName
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
        }
    }

    public IList<Vaccine> GetAllVaccinesByAnimalId(int animalId)
    {
        using (var connection = CreateConnection())
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
                            a.nome AS AnimalName,
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
        }
    }

    public Vaccine GetVaccineById(int id)
    {
        using (var connection = CreateConnection())
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
                            a.nome AS AnimalName,
                            a.data_nascimento AS DateOfBirth,
                            a.sexo AS Sex,
                            a.raca AS Race,
                            a.peso AS Weight,
                            a.vivo AS Alive,
                            e.id AS SpecieId,
                            e.nome AS SpecieName
                        FROM vacinas v
                        INNER JOIN cartilhas_vacinacao c ON c.id = v.id_cartilha_vacinacao
                        INNER JOIN animais a ON a.id = v.id_animal
                        INNER JOIN especies e ON e.id = a.id_especie
                        WHERE v.id = @VaccineId";

                var result = connection.Query<Vaccine, VaccinationSchedule, Animal, Specie, Vaccine>(
                    sql,
                    (vaccine, vaccinationSchedule, animal, specie) =>
                    {
                        vaccine.Animal = animal;
                        vaccine.VaccinationSchedule = vaccinationSchedule;
                        vaccine.Animal.Specie = specie;
                        return vaccine;
                    },
                    new { VaccineId = id },
                    splitOn: "VaccinationScheduleId, AnimalId, SpecieId"
                ).FirstOrDefault();

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public int DeleteVaccineById(int id)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = "DELETE FROM vacinas WHERE id = @VaccineId";
                return connection.Execute(query, new { VaccineId = id });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public int NewVaccine(Vaccine vaccine)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = @"
                        INSERT INTO vacinas(data_administracao, lote, fabricante, data_fabricacao, id_animal, id_cartilha_vacinacao)
                        VALUES(@DateOfAdministration, @Lot, @Manufacturer, @DateOfManufacture, @AnimalId, @VaccinationScheduleId)
                        RETURNING id;";

                var parameters = new
                {
                    DateOfAdministration = vaccine.DateOfAdministration,
                    Lot = vaccine.Lot,
                    Manufacturer = vaccine.Manufacturer,
                    DateOfManufacture = vaccine.DateOfManufacture,
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
        }
    }

    public int UpdateVaccine(Vaccine vaccine)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = @"
                        UPDATE vacinas
                        SET data_administracao = @DateOfAdministration,
                            lote = @Lot,
                            fabricante = @Manufacturer,
                            data_fabricacao = @DateOfManufacture,
                            id_cartilha_vacinacao = @VaccinationScheduleId
                        WHERE id = @VaccineId";

                var parameters = new
                {
                    DateOfAdministration = vaccine.DateOfAdministration,
                    Lot = vaccine.Lot,
                    Manufacturer = vaccine.Manufacturer,
                    DateOfManufacture = vaccine.DateOfManufacture,
                    VaccinationScheduleId = vaccine.VaccinationSchedule.VaccinationScheduleId,
                    VaccineId = vaccine.VaccineId
                };

                return connection.Execute(query, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
