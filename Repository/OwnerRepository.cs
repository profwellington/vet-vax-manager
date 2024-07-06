using Dapper;
using VetVaxManager.Models;

namespace VetVaxManager.Repository;

public class OwnerRepository : BaseRepository, IOwnerRepository
{
    public OwnerRepository(IConfiguration configuration) : base(configuration)
    {
    }

    public Owner GetOwnerById(int id)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                string sql = @"
                    SELECT 
                        p.id AS OwnerId,
                        p.nome AS Name,
                        p.sobrenome AS LastName,
                        p.data_nascimento AS DateOfBirth,
                        p.sexo AS Sex,
                        p.cpf AS Cpf,
                        p.email AS Email,
                        p.telefone AS Phone
                    FROM 
                        proprietarios p
                    WHERE 
                        p.id = @OwnerId";

                var result = connection.QueryFirstOrDefault<Owner>(sql, new { OwnerId = id });

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public int UpdateOwner(Owner owner)
    {
        using (var connection = CreateConnection())
        {
            try
            {
                connection.Open();
                var query = @"
                                UPDATE proprietarios
                                SET nome = @Name,
                                    sobrenome = @LastName,
                                    data_nascimento = @DateOfBirth,
                                    sexo = @Sex,
                                    cpf = @Cpf,
                                    email = @Email,
                                    telefone = @Phone
                                WHERE id = @OwnerId";
                var parameters = new
                {
                    Name = owner.Name,
                    LastName = owner.LastName,
                    DateOfBirth = owner.DateOfBirth,
                    Sex = owner.Sex,
                    Cpf = owner.Cpf,
                    Email = owner.Email,
                    Phone = owner.Phone,
                    OwnerId = owner.OwnerId
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
