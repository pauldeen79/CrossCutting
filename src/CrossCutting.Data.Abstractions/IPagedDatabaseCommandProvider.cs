namespace CrossCutting.Data.Abstractions;

public interface IPagedDatabaseCommandProvider
{
    Task<Result<IPagedDatabaseCommand>> CreatePagedAsync<TSource>(DatabaseOperation operation, int offset, int pageSize, CancellationToken token);
}

public interface IPagedDatabaseCommandProvider<in TSource>
{
    Task<Result<IPagedDatabaseCommand>> CreatePagedAsync(TSource source, DatabaseOperation operation, int offset, int pageSize, CancellationToken token);
}
