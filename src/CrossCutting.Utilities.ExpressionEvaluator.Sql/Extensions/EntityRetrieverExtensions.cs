using CrossCutting.Data.Abstractions.Extensions;

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
        IFieldNameProvider fieldNameProvider,CancellationToken token)
        where T : class
    {
        return await (await evaluatableSqlExpressionProvider.GetExpressionAsync(settings, condition, fieldNameProvider, token).ConfigureAwait(false))
                .OnSuccessAsync(builder => instance.FindManyAsync(builder, token)).ConfigureAwait(false);
    }

    //TODO: Review if we need to add support for find paged. I need two SelectCommandBuilders, one for the COUNT(*) and one for the select itself (where we need two additional parameters: skip and take) But I guess this is not logical because the condition/evaluatable should include the offset and length?
    // public static async Task<Result<IPagedResult<T>>> FindPagedAsync<T>(
    //     this IDatabaseEntityRetriever<T> instance,
    //     IEvaluatableSqlExpressionProvider evaluatableSqlExpressionProvider,
    //     IPagedDatabaseEntityRetrieverSettings settings,
    //     IEvaluatable<bool> condition,
    //     IFieldNameProvider fieldNameProvider,CancellationToken token)
    //     where T : class
    // {
    //     return await (await evaluatableSqlExpressionProvider.GetExpressionAsync(settings, condition, fieldNameProvider, token).ConfigureAwait(false))
    //             .OnSuccessAsync(builder => instance.FindPagedAsync(builder, token)).ConfigureAwait(false);
    // }
}