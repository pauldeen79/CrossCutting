namespace CrossCutting.Data.Abstractions;

public interface IDatabaseCommandProvider
{
    Task<Result<IDatabaseCommand>> CreateAsync<TSource>(DatabaseOperation operation);
}

public interface IDatabaseCommandProvider<in TSource>
{
    Task<Result<IDatabaseCommand>> CreateAsync(TSource source, DatabaseOperation operation);
}
