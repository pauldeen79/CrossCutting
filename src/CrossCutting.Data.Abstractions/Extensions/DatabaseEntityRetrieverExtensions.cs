namespace CrossCutting.Data.Abstractions.Extensions;

public static class DatabaseEntityRetrieverExtensions
{
    public static Task<T?> FindOneAsync<T>(this IDatabaseEntityRetriever<T> retriever, IDatabaseCommand command)
        where T : class
        => retriever.FindOneAsync(command, CancellationToken.None);

    public static Task<IReadOnlyCollection<T>> FindManyAsync<T>(this IDatabaseEntityRetriever<T> retriever, IDatabaseCommand command)
        where T : class
        => retriever.FindManyAsync(command, CancellationToken.None);

    public static Task<IPagedResult<T>> FindPagedAsync<T>(this IDatabaseEntityRetriever<T> retriever, IPagedDatabaseCommand command)
        where T : class
        => retriever.FindPagedAsync(command, CancellationToken.None);
}
