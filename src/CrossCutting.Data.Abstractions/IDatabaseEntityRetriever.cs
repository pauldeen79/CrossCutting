namespace CrossCutting.Data.Abstractions;

public interface IDatabaseEntityRetriever<T> where T : class
{
    Task<Result<T>> FindOneAsync(IDatabaseCommand command, CancellationToken cancellationToken);
    Task<Result<IReadOnlyCollection<T>>> FindManyAsync(IDatabaseCommand command, CancellationToken cancellationToken);
    Task<Result<IPagedResult<T>>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken cancellationToken);
}
