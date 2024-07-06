using System.Data;

namespace VetVaxManager.Repository;

public abstract class BaseRepository
{
    private readonly IConfiguration _configuration;

    protected BaseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    protected IDbConnection CreateConnection()
    {
        var connectionString = _configuration.GetSection("ConnectionStrings").GetSection("PostgresConnection").Value;
        return new Npgsql.NpgsqlConnection(connectionString);
    }
}
