using System.Data;

namespace Foundation.Data;

public static class DbConnectionExtensions
{
    public static IDbConnection OpenIfClosed(this IDbConnection connection)
    {
        connection.ThrowIfNull();
        if (connection.State != ConnectionState.Open) connection.Open();

        return connection;
    }
}
