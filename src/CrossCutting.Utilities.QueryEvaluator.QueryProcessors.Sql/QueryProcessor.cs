namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryProcessor : IQueryProcessor
{
    private readonly IPagedDatabaseCommandProvider<IQuery> _pagedDatabaseCommandProvider;
    private readonly IEnumerable<IDatabaseEntityRetrieverProvider> _databaseEntityRetrieverProviders;

    public QueryProcessor(
        IPagedDatabaseCommandProvider<IQuery> pagedDatabaseCommandProvider,
        IEnumerable<IDatabaseEntityRetrieverProvider> databaseEntityRetrieverProviders)
    {
        ArgumentGuard.IsNotNull(pagedDatabaseCommandProvider, nameof(pagedDatabaseCommandProvider));
        ArgumentGuard.IsNotNull(databaseEntityRetrieverProviders, nameof(databaseEntityRetrieverProviders));

        _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;
        _databaseEntityRetrieverProviders = databaseEntityRetrieverProviders;
    }

    public async Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IQuery query, CancellationToken cancellationToken) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return await GetDatabaseEntityRetriever<TResult>(query)
            .OnSuccessAsync(async provider => await provider.FindManyAsync(CreateCommand(query, query.Limit.GetValueOrDefault()).DataCommand, cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    public async Task<Result<TResult>> FindOneAsync<TResult>(IQuery query, CancellationToken cancellationToken) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return await GetDatabaseEntityRetriever<TResult>(query)
            .OnSuccessAsync(async provider =>
            {
                var item = await provider.FindOneAsync(CreateCommand(query, 1).DataCommand, cancellationToken).ConfigureAwait(false);
                return item is null
                    ? Result.NotFound<TResult>()
                    : Result.Success(item!);
            })
            .ConfigureAwait(false);
    }

    public async Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IQuery query, CancellationToken cancellationToken) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return await GetDatabaseEntityRetriever<TResult>(query)
            .OnSuccessAsync(async provider => await provider.FindPagedAsync(CreateCommand(query, query.Limit.GetValueOrDefault()), cancellationToken).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private IPagedDatabaseCommand CreateCommand(IQuery query, int pageSize)
        => _pagedDatabaseCommandProvider.CreatePaged(query.ThrowOnInvalid(), DatabaseOperation.Select, query.Offset ?? 0, pageSize);

    private Result<IDatabaseEntityRetriever<TResult>> GetDatabaseEntityRetriever<TResult>(IQuery query) where TResult : class
        => _databaseEntityRetrieverProviders
            .Select(x => x.Create<TResult>(query))
            .WhenNotContinue(() => Result.NotFound<IDatabaseEntityRetriever<TResult>>($"Could not find entity retriever for query of type {typeof(TResult).FullName}"));
}
