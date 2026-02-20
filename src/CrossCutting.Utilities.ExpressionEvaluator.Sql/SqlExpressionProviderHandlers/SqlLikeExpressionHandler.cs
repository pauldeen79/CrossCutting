namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.SqlExpressionProviderHandlers;

public class SqlLikeExpressionHandler : SqlExpressionProviderHandlerBase<SqlLikeExpression>
{
    protected override async Task<Result<string>> HandleGetSqlExpressionAsync(object? context, SqlLikeExpression expression, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, ISqlExpressionProvider callback, CancellationToken token)
    {
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldNameProvider = ArgumentGuard.IsNotNull(fieldNameProvider, nameof(fieldNameProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        callback = ArgumentGuard.IsNotNull(callback, nameof(callback));

        return (await callback.GetSqlExpressionAsync(context, new SqlExpression(expression.SourceExpression), fieldNameProvider, parameterBag, token).ConfigureAwait(false))
            .OnSuccess(value => parameterBag.ReplaceString(value, expression.FormatString));
    }
}
