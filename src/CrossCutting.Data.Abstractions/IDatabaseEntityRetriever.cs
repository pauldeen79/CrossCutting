namespace CrossCutting.Data.Abstractions;

public interface IDatabaseEntityRetriever<T> where T : class
{
    T? FindOne(IDatabaseCommand command);
    Task<T?> FindOneAsync(IDatabaseCommand command, CancellationToken cancellationToken);
    IReadOnlyCollection<T> FindMany(IDatabaseCommand command);
    Task<IReadOnlyCollection<T>> FindManyAsync(IDatabaseCommand command, CancellationToken cancellationToken);
    IPagedResult<T> FindPaged(IPagedDatabaseCommand command);
    Task<IPagedResult<T>> FindPagedAsync(IPagedDatabaseCommand command, CancellationToken cancellationToken);
}
