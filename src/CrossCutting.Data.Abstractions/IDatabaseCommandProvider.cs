namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommandProvider
{
    Result<IDatabaseCommand> Create<TSource>(DatabaseOperation operation);
}

public interface IDatabaseCommandProvider<in TSource>
{
    Result<IDatabaseCommand> Create(TSource source, DatabaseOperation operation);
}
