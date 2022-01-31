namespace CrossCutting.Data.Sql.Extensions;

internal static class DbConnectionExtensions
{
    internal static void OpenIfNecessary(this IDbConnection connection)
    {
        if (connection.State == ConnectionState.Closed)
        {
            connection.Open();
        }
    }
}
