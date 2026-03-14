namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Extensions;

public static class EvaluatableSqlExpressionProviderExtensions
{
    public static Task<Result<IDatabaseCommand>> GetExpressionAsync(
        this IEvaluatableSqlExpressionProvider instance,
        IPagedDatabaseEntityRetrieverSettings settings,
        IEvaluatable<bool> condition,
        IFieldNameProvider fieldNameProvider,
        CancellationToken token)
        => instance.GetExpressionAsync(settings, null, condition, fieldNameProvider, token);

    public static async Task<Result<IDatabaseCommand>> GetExpressionAsync(
        this IEvaluatableSqlExpressionProvider instance,
        IPagedDatabaseEntityRetrieverSettings settings,
        object? context,
        IEvaluatable<bool> condition,
        IFieldNameProvider fieldNameProvider,
        CancellationToken token)
    {
        settings = ArgumentGuard.IsNotNull(settings, nameof(settings));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldNameProvider = ArgumentGuard.IsNotNull(fieldNameProvider, nameof(fieldNameProvider));

        var builder = new SelectCommandBuilder()
                .Select(settings.Fields)
                .From(settings.TableName)
                .OrderBy(settings.DefaultOrderBy);

        return (await instance.GetExpressionAsync(builder, context, condition, fieldNameProvider, token).ConfigureAwait(false))
            .OnSuccess(_ => Result.Success(builder.Build()));
    }
}