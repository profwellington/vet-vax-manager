using Dapper;
using MySql.Data.MySqlClient;
using VetVaxManager.Models;

namespace VetVaxManager.Repository
{
    public class UserRepository : IUserRepository
    {
        IConfiguration _configuration;
        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionStrings").GetSection("MySQLConnection").Value;
            return connection;
        }
        public int NewUser(User user)
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    var queryOwner = @"INSERT INTO proprietarios (nome, sobrenome, data_nascimento, sexo, cpf, email, telefone) 
                                            VALUES (@Name, @LastName, @DateOfBirth, @Sex, @Cpf, @Email, @Phone); 
                                            SELECT LAST_INSERT_ID();";

                    var parametersOwner = new
                    {
                        Name = user.Owner.Name,
                        LastName = user.Owner.LastName,
                        DateOfBirth = user.Owner.DateOfBirth,
                        Sex = user.Owner.Sex,
                        Cpf = user.Owner.Cpf,
                        Email = user.Email,
                        Phone = user.Owner.Phone
                    };

                    user.Owner.OwnerId = connection.QuerySingle<int>(queryOwner, parametersOwner);

                    var queryUser = @"
                                INSERT INTO usuarios(email, senha, id_proprietario)
                                VALUES(@Email, @Password, @OwnerId);
                                SELECT LAST_INSERT_ID();";

                    var parametersUser = new
                    {
                        Email = user.Email,
                        Password = user.Password,
                        OwnerId = user.Owner.OwnerId,
                    };

                    int id = connection.QuerySingle<int>(queryUser, parametersUser);

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

        public User GetByEmail(string email)
        {
            var connectionString = this.GetConnection();
            using (var connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string sql = @"
                    SELECT
                        u.id AS UserId,
                        u.email AS Email,
                        u.senha AS Password,
                        p.id AS OwnerId,
                        p.nome AS Name,
                        p.sobrenome AS LastName,
                        p.data_nascimento AS DateOfBirth,
                        p.sexo AS Sex,
                        p.cpf AS Cpf,
                        p.telefone AS Phone
                    FROM usuarios u
                    INNER JOIN proprietarios p ON p.id = u.id_proprietario
                    WHERE u.email = @Email";

                    var result = connection.Query<User, Owner, User>(
                        sql,
                        (user, owner) =>
                        {
                            user.Owner = owner;
                            return user;
                        },
                        new { Email = email },
                        splitOn: "OwnerId"
                    ).SingleOrDefault();

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
