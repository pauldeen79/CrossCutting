namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql;

public class QueryProcessor : IQueryProcessor
{
    private readonly IPagedDatabaseCommandProvider<IQueryContext> _pagedDatabaseCommandProvider;
    private readonly IEnumerable<IDatabaseEntityRetrieverProvider> _databaseEntityRetrieverProviders;

    public QueryProcessor(
        IPagedDatabaseCommandProvider<IQueryContext> pagedDatabaseCommandProvider,
        IEnumerable<IDatabaseEntityRetrieverProvider> databaseEntityRetrieverProviders)
    {
        ArgumentGuard.IsNotNull(pagedDatabaseCommandProvider, nameof(pagedDatabaseCommandProvider));
        ArgumentGuard.IsNotNull(databaseEntityRetrieverProviders, nameof(databaseEntityRetrieverProviders));

        _pagedDatabaseCommandProvider = pagedDatabaseCommandProvider;
        _databaseEntityRetrieverProviders = databaseEntityRetrieverProviders;
    }

    public async Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IQuery query, object? context, CancellationToken token) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return await GetDatabaseEntityRetriever<TResult>(query)
            .OnSuccessAsync(async provider =>
                await Result.WrapExceptionAsync(async () =>
                {
                    var commandResult = (await CreateCommandAsync(query, context, query.Limit.GetValueOrDefault(), token).ConfigureAwait(false))
                        .EnsureValue();
                    if (!commandResult.IsSuccessful())
                    {
                        return Result.FromExistingResult<IReadOnlyCollection<TResult>>(commandResult);
                    }

                    return await provider.FindManyAsync(commandResult.Value!.DataCommand, token).ConfigureAwait(false);
                }).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    public async Task<Result<TResult>> FindOneAsync<TResult>(IQuery query, object? context, CancellationToken token) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return await GetDatabaseEntityRetriever<TResult>(query)
            .OnSuccessAsync(async provider =>
                await Result.WrapExceptionAsync(async () =>
                {
                    var commandResult = (await CreateCommandAsync(query, context, 1, token).ConfigureAwait(false))
                        .EnsureValue();
                    if (!commandResult.IsSuccessful())
                    {
                        return Result.FromExistingResult<TResult>(commandResult);
                    }

                    return await provider.FindOneAsync(commandResult.Value!.DataCommand, token).ConfigureAwait(false);
                }).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

    public async Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IQuery query, object? context, CancellationToken token) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        return await GetDatabaseEntityRetriever<TResult>(query)
            .OnSuccessAsync(async provider =>
                await Result.WrapExceptionAsync(async () =>
                {
                    var commandResult = (await CreateCommandAsync(query, context, query.Limit.GetValueOrDefault(), token).ConfigureAwait(false))
                        .EnsureValue();
                    if (!commandResult.IsSuccessful())
                    {
                        return Result.FromExistingResult<IPagedResult<TResult>>(commandResult);
                    }

                    return await provider.FindPagedAsync(commandResult.Value!, token).ConfigureAwait(false);
                }).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private async Task<Result<IPagedDatabaseCommand>> CreateCommandAsync(IQuery query, object? context, int pageSize, CancellationToken token)
        => await _pagedDatabaseCommandProvider.CreatePagedAsync(query.EnsureValid().WithContext(context), DatabaseOperation.Select, query.Offset ?? 0, pageSize, token).ConfigureAwait(false);

    private Result<IDatabaseEntityRetriever<TResult>> GetDatabaseEntityRetriever<TResult>(IQuery query) where TResult : class
        => _databaseEntityRetrieverProviders
            .Select(x => x.Create<TResult>(query))
            .WhenNotContinue(() => Result.NotFound<IDatabaseEntityRetriever<TResult>>($"Could not find entity retriever for query of type {typeof(TResult).FullName}"));
}
