namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlConditionExpressionProvider
{
    Task<Result<string>> GetConditionExpressionAsync(IQueryContext context,
                                                     ICondition condition,
                                                     IQueryFieldInfo fieldInfo,
                                                     ISqlExpressionProvider sqlExpressionProvider,
                                                     ParameterBag parameterBag);
}
