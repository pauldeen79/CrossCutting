namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class SqlLikeExpressionHandler : SqlExpressionProviderHandlerBase<SqlLikeExpression>
{
    protected override async Task<Result<string>> DoGetSqlExpressionAsync(IQueryContext context, SqlLikeExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback, CancellationToken token)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        callback = ArgumentGuard.IsNotNull(callback, nameof(callback));

        return (await callback.GetSqlExpressionAsync(context, new SqlExpression(expression.SourceExpression), fieldInfo, parameterBag, token).ConfigureAwait(false))
            .OnSuccess(value => parameterBag.ReplaceString(value, expression.FormatString));
    }
}
