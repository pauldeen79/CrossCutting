namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlConditionExpressionProviderHandler
{
    Task<Result> GetConditionExpressionAsync(StringBuilder builder,
                                             IQueryContext context,
                                             ICondition condition,
                                             IQueryFieldInfo fieldInfo,
                                             ISqlExpressionProvider sqlExpressionProvider,
                                             ParameterBag parameterBag);
}
