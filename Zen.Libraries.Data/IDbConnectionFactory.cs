using System.Data.Common;

namespace Zen.Libraries.Data;

public interface IDbConnectionFactory
{
    DbConnection CreateConnection();
}