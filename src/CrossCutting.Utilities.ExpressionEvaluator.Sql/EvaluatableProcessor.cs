namespace CrossCutting.Utilities.ExpressionEvaluator.Sql;

public class EvaluatableProcessor(
    IPagedDatabaseCommandProvider<IEvaluatableContext> pagedDatabaseCommandProvider,
    IEnumerable<IDatabaseEntityRetrieverProvider> databaseEntityRetrieverProviders) : IEvaluatableProcessor
{
    private readonly IPagedDatabaseCommandProvider<IEvaluatableContext> _pagedDatabaseCommandProvider = ArgumentGuard.IsNotNull(pagedDatabaseCommandProvider, nameof(pagedDatabaseCommandProvider));
    private readonly IDatabaseEntityRetrieverProvider[] _databaseEntityRetrieverProviders = ArgumentGuard.IsNotNull(databaseEntityRetrieverProviders, nameof(databaseEntityRetrieverProviders)).ToArray();

    public async Task<Result<IReadOnlyCollection<TResult>>> FindManyAsync<TResult>(IEvaluatable<bool> evaluatable, object? context, CancellationToken token) where TResult : class
    {
        evaluatable = ArgumentGuard.IsNotNull(evaluatable, nameof(evaluatable));

        return await GetDatabaseEntityRetriever<TResult>(evaluatable)
            .OnSuccessAsync(async provider =>
                await Result.WrapExceptionAsync(async () =>
                {
                    var commandResult = (await CreateCommandAsync(evaluatable, typeof(TResult), context, 0, 0, token).ConfigureAwait(false))
                        .EnsureValue();
                    if (!commandResult.IsSuccessful())
                    {
                        return Result.FromExistingResult<IReadOnlyCollection<TResult>>(commandResult);
                    }

                    return await provider.FindManyAsync(commandResult.Value!.DataCommand, token).ConfigureAwait(false);
                }).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    public async Task<Result<TResult>> FindOneAsync<TResult>(IEvaluatable<bool> evaluatable, object? context, CancellationToken token) where TResult : class
    {
        evaluatable = ArgumentGuard.IsNotNull(evaluatable, nameof(evaluatable));

        return await GetDatabaseEntityRetriever<TResult>(evaluatable)
            .OnSuccessAsync(async provider =>
                await Result.WrapExceptionAsync(async () =>
                {
                    var commandResult = (await CreateCommandAsync(evaluatable, typeof(TResult), context, 1, 0, token).ConfigureAwait(false))
                        .EnsureValue();
                    if (!commandResult.IsSuccessful())
                    {
                        return Result.FromExistingResult<TResult>(commandResult);
                    }

                    return await provider.FindOneAsync(commandResult.Value!.DataCommand, token).ConfigureAwait(false);
                }).ConfigureAwait(false)
            ).ConfigureAwait(false);
    }

    public async Task<Result<IPagedResult<TResult>>> FindPagedAsync<TResult>(IEvaluatable<bool> evaluatable, int offset, int pageSize, object? context, CancellationToken token) where TResult : class
    {
        evaluatable = ArgumentGuard.IsNotNull(evaluatable, nameof(evaluatable));

        return await GetDatabaseEntityRetriever<TResult>(evaluatable)
            .OnSuccessAsync(async provider =>
                await Result.WrapExceptionAsync(async () =>
                {
                    var commandResult = (await CreateCommandAsync(evaluatable, typeof(TResult), context, offset, pageSize, token).ConfigureAwait(false))
                        .EnsureValue();
                    if (!commandResult.IsSuccessful())
                    {
                        return Result.FromExistingResult<IPagedResult<TResult>>(commandResult);
                    }

                    return await provider.FindPagedAsync(commandResult.Value!, token).ConfigureAwait(false);
                }).ConfigureAwait(false))
            .ConfigureAwait(false);
    }

    private async Task<Result<IPagedDatabaseCommand>> CreateCommandAsync(IEvaluatable<bool> evaluatable, Type entityType, object? context, int offset, int pageSize, CancellationToken token)
    {
        var validationResult = Result.FromValidatableInstance(evaluatable);
        if (!validationResult.IsSuccessful())
        {
            return Result.FromExistingResult<IPagedDatabaseCommand>(validationResult); 
        }

        return await _pagedDatabaseCommandProvider.CreatePagedAsync(evaluatable.WithTypeAndContext(entityType, context), DatabaseOperation.Select, offset, pageSize, token)
            .ConfigureAwait(false);
    }

    private Result<IDatabaseEntityRetriever<TResult>> GetDatabaseEntityRetriever<TResult>(IEvaluatable<bool> evaluatable) where TResult : class
        => _databaseEntityRetrieverProviders
            .Select(x => x.Create<TResult>(evaluatable))
            .WhenNotContinue(() => Result.NotFound<IDatabaseEntityRetriever<TResult>>($"Could not find entity retriever for query of type {typeof(TResult).FullName}"));
}