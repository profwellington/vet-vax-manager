using Dapper;
using MySql.Data.MySqlClient;
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

        public Owner GetOwnerById(int id)
        {
            var connectionString = this.GetConnection();

            using (var connection = new MySqlConnection(connectionString))
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

                    var result = connection.Query<Owner>(
                        sql,
                        new { OwnerId = id }
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

        public int UpdateOwner(Owner owner)
        {
            var connectionString = this.GetConnection();
            var count = 0;
            using (var connection = new MySqlConnection(connectionString))
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

                    count = connection.Execute(query, parameters);
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
    }
}
