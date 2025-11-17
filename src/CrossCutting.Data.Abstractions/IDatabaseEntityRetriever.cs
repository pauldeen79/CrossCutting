namespace CrossCutting.Data.Abstractions;

public interface IDatabaseEntityRetriever<T> where T : class
{
    Task<Result<T>> FindOneAsync(IDatabaseCommand command, CancellationToken token);
    Task<Result<IReadOnlyCollection<T>>> FindManyAsync(IDatabaseCommand command, CancellationToken token);
    Task<Result<IPagedResult<T>>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken token);
}
