namespace CrossCutting.Data.Sql.Abstractions;

public interface ISqlConnectionWrapperFactory
{
    SqlConnectionWrapper Create(IDbConnection connection);
}
