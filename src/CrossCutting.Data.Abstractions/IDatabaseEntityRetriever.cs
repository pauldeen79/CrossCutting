namespace CrossCutting.Data.Abstractions;

public interface IDatabaseEntityRetriever<T> where T : class
{
    Result<T> FindOne(IDatabaseCommand command);
    Task<Result<T>> FindOneAsync(IDatabaseCommand command, CancellationToken cancellationToken);
    Result<IReadOnlyCollection<T>> FindMany(IDatabaseCommand command);
    Task<Result<IReadOnlyCollection<T>>> FindManyAsync(IDatabaseCommand command, CancellationToken cancellationToken);
    Result<IPagedResult<T>> FindPaged(IPagedDatabaseCommand command);
    Task<Result<IPagedResult<T>>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken cancellationToken);
}
