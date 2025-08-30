namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviderHandlers;

public abstract class ConditionExpressionHandlerBase<TCondition> : ISqlConditionExpressionProviderHandler
{
    public Result GetConditionExpression(
        StringBuilder builder,
        IQuery query,
        ICondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag)
    {
        builder = ArgumentGuard.IsNotNull(builder, nameof(builder));
        query = ArgumentGuard.IsNotNull(query, nameof(query));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));
        fieldInfo = ArgumentGuard.IsNotNull(fieldInfo, nameof(fieldInfo));
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        parameterBag = ArgumentGuard.IsNotNull(parameterBag, nameof(parameterBag));

        if (condition is not TCondition typedCondition)
        {
            return Result.Continue();
        }

        return DoGetConditionExpression(builder, query, typedCondition, fieldInfo, sqlExpressionProvider, parameterBag);
    }

    protected abstract Result DoGetConditionExpression(
        StringBuilder builder,
        IQuery query,
        TCondition condition,
        IQueryFieldInfo fieldInfo,
        ISqlExpressionProvider sqlExpressionProvider,
        ParameterBag parameterBag);
}
