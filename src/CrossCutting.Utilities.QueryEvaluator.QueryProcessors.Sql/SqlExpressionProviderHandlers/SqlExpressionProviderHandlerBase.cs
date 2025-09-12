namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlExpressionProviderHandlers;

public abstract class SqlExpressionProviderHandlerBase<TExpression> : ISqlExpressionProviderHandler
{
    public Result<string> GetSqlExpression(
        IQueryContext context,
        IExpression expression,
        IQueryFieldInfo fieldInfo,
        ParameterBag parameterBag,
        ISqlExpressionProvider callback)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        expression = ArgumentGuard.IsNotNull(expression, nameof(expression));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (expression is not TExpression typedExpression)
        {
            return Result.Continue<string>();
        }

        return DoGetSqlExpression(context, typedExpression, fieldInfo, parameterBag, callback);
    }

    protected abstract Result<string> DoGetSqlExpression(
        IQueryContext context,
        TExpression expression,
        IQueryFieldInfo fieldInfo,
        ParameterBag parameterBag,
        ISqlExpressionProvider callback);
}
