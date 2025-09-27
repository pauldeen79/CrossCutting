namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public class SqlLikeExpressionHandler : SqlExpressionProviderHandlerBase<SqlLikeEvaluatable>
{
    protected override Result<string> DoGetSqlExpression(IQueryContext context, SqlLikeEvaluatable expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback)
    {
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        callback = ArgumentGuard.IsNotNull(callback, nameof(callback));

        return callback.GetSqlExpression(context, expression.SourceExpression, fieldInfo, parameterBag)
            .OnSuccess(value => parameterBag.ReplaceString(value, expression.FormatString));
    }
}
