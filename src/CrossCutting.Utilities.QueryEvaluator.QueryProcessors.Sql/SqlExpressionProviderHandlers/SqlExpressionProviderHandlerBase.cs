namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public abstract class SqlExpressionProviderHandlerBase<TExpression> : ISqlExpressionProviderHandler
{
    public Result<string> GetSqlExpression(
        IQuery query,
        object? context,
        IExpression expression,
        IQueryFieldInfo fieldInfo,
        ParameterBag parameterBag,
        ISqlExpressionProvider callback)
    {
        query = ArgumentGuard.IsNotNull(query, nameof(query));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (expression is not TExpression typedExpression)
        {
            return Result.Continue<string>();
        }

        return DoGetSqlExpression(query, context, typedExpression, fieldInfo, parameterBag, callback);
    }

    protected abstract Result<string> DoGetSqlExpression(
        IQuery query,
        object? context,
        TExpression expression,
        IQueryFieldInfo fieldInfo,
        ParameterBag parameterBag,
        ISqlExpressionProvider callback);
}
