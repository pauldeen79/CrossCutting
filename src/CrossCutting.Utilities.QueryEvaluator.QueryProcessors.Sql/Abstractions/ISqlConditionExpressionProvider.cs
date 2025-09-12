namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlConditionExpressionProvider
{
    Result<string> GetConditionExpression(IQueryContext context,
                                          ICondition condition,
                                          IQueryFieldInfo fieldInfo,
                                          ISqlExpressionProvider sqlExpressionProvider,
                                          ParameterBag parameterBag);
}
