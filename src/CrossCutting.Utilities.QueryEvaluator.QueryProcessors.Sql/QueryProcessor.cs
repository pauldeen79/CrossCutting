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

    public async Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IQuery query, object? context, CancellationToken cancellationToken) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        var entityRetrieverResult = GetDatabaseEntityRetriever<TResult>(query).EnsureValue();
        if (!entityRetrieverResult.IsSuccessful())
        {
            return Result.FromExistingResult<IReadOnlyCollection<TResult>>(entityRetrieverResult);
        }
        
        var commandResult = (await CreateCommandAsync(query, context, query.Limit.GetValueOrDefault()).ConfigureAwait(false)).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<IReadOnlyCollection<TResult>>(commandResult);
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return await entityRetrieverResult.Value!.FindManyAsync(commandResult.Value!.DataCommand, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result.Error<IReadOnlyCollection<TResult>>(ex, "Exception occured in FindMany operation");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    public async Task<Result<TResult>> FindOneAsync<TResult>(IQuery query, object? context, CancellationToken cancellationToken) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        var entityRetrieverResult = GetDatabaseEntityRetriever<TResult>(query).EnsureValue();
        if (!entityRetrieverResult.IsSuccessful())
        {
            return Result.FromExistingResult<TResult>(entityRetrieverResult);
        }

        var commandResult = (await CreateCommandAsync(query, context, query.Limit.GetValueOrDefault()).ConfigureAwait(false)).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<TResult>(commandResult);
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return await entityRetrieverResult.Value!.FindOneAsync(commandResult.Value!.DataCommand, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result.Error<TResult>(ex, "Exception occured in FindOne operation");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    public async Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IQuery query, object? context, CancellationToken cancellationToken) where TResult : class
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));

        var entityRetrieverResult = GetDatabaseEntityRetriever<TResult>(query).EnsureValue();
        if (!entityRetrieverResult.IsSuccessful())
        {
            return Result.FromExistingResult<IPagedResult<TResult>>(entityRetrieverResult);
        }

        var commandResult = (await CreateCommandAsync(query, context, query.Limit.GetValueOrDefault()).ConfigureAwait(false)).EnsureValue();
        if (!commandResult.IsSuccessful())
        {
            return Result.FromExistingResult<IPagedResult<TResult>>(commandResult);
        }

#pragma warning disable CA1031 // Do not catch general exception types
        try
        {
            return await entityRetrieverResult.Value!.FindPagedAsync(commandResult.Value!, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return Result.Error<IPagedResult<TResult>>(ex, "Exception occured in FindOne operation");
        }
#pragma warning restore CA1031 // Do not catch general exception types
    }

    private Task<Result<IPagedDatabaseCommand>> CreateCommandAsync(IQuery query, object? context, int pageSize)
        => _pagedDatabaseCommandProvider.CreatePagedAsync(query.EnsureValid().WithContext(context), DatabaseOperation.Select, query.Offset ?? 0, pageSize);

    private Result<IDatabaseEntityRetriever<TResult>> GetDatabaseEntityRetriever<TResult>(IQuery query) where TResult : class
        => _databaseEntityRetrieverProviders
            .Select(x => x.Create<TResult>(query))
            .WhenNotContinue(() => Result.NotFound<IDatabaseEntityRetriever<TResult>>($"Could not find entity retriever for query of type {typeof(TResult).FullName}"));
}
