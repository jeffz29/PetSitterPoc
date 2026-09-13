using System.Data.Common;
using Npgsql;

namespace Zen.Libraries.Data;

public sealed class NpgsqlConnectionFactory(string connectionString) : IDbConnectionFactory
{
    public DbConnection CreateConnection() => new NpgsqlConnection(connectionString);
}