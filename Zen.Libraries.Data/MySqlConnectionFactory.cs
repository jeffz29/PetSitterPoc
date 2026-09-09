using System.Data.Common;
using MySqlConnector;

namespace Zen.Libraries.Data;

public sealed class MySqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public DbConnection CreateConnection() => new MySqlConnection(connectionString);
}