namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlConditionExpressionProviderHandler
{
    Result GetConditionExpression(StringBuilder builder,
                                  IQuery query,
                                  ICondition condition,
                                  IQueryFieldInfo fieldInfo,
                                  ISqlExpressionProvider sqlExpressionProvider,
                                  ParameterBag parameterBag);
}
