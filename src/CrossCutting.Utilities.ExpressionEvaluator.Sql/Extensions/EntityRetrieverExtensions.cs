namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class EntityRetrieverExtensions
{
    public static async Task<Result<T>> FindOneAsync<T>(
        this IDatabaseEntityRetriever<T> instance,
        IEvaluatableSqlExpressionProvider evaluatableSqlExpressionProvider,
        IPagedDatabaseEntityRetrieverSettings settings,
        IEvaluatable<bool> condition,
        IFieldNameProvider fieldNameProvider,CancellationToken token)
        where T : class
    {
        return await (await evaluatableSqlExpressionProvider.GetExpressionAsync(settings, condition, fieldNameProvider, token).ConfigureAwait(false))
                .OnSuccessAsync(builder => instance.FindOneAsync(builder, token)).ConfigureAwait(false);
    }

    public static async Task<Result<IReadOnlyCollection<T>>> FindManyAsync<T>(
        this IDatabaseEntityRetriever<T> instance,
        IEvaluatableSqlExpressionProvider evaluatableSqlExpressionProvider,
        IPagedDatabaseEntityRetrieverSettings settings,
        IEvaluatable<bool> condition,
        IFieldNameProvider fieldNameProvider,
        CancellationToken token)
        where T : class
    {
        return await (await evaluatableSqlExpressionProvider.GetExpressionAsync(settings, condition, fieldNameProvider, token).ConfigureAwait(false))
                .OnSuccessAsync(databaseCommand => instance.FindManyAsync(databaseCommand, token)).ConfigureAwait(false);
    }

    public static async Task<Result<IPagedResult<T>>> FindPagedAsync<T>(
        this IDatabaseEntityRetriever<T> instance,
        IEvaluatableSqlExpressionProvider evaluatableSqlExpressionProvider,
        IPagedDatabaseEntityRetrieverSettings settings,
        IEvaluatable<bool> condition,
        IFieldNameProvider fieldNameProvider,
        int offset,
        int pageSize,
        CancellationToken token)
        where T : class
    {
        return await (await evaluatableSqlExpressionProvider.GetExpressionAsync(settings, condition, fieldNameProvider, token).ConfigureAwait(false))
                .OnSuccessAsync(databaseCommand => instance.FindPagedAsync(databaseCommand.ToPagedCommand(offset, pageSize), token)).ConfigureAwait(false);
    }
}