namespace CrossCutting.Data.Sql.Abstractions;

public interface ISqlCommandWrapperFactory
{
    SqlCommandWrapper Create(IDbCommand command);
}
