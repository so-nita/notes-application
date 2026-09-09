using System.Data;
using Microsoft.Data.SqlClient;

namespace NoteApplication.API.Contexts;

public interface IAppDataContext
{
    public IDbConnection CreateConnection();
}

public class AppDataContext : IAppDataContext
{
    private readonly string _connectionString;

    public AppDataContext(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}