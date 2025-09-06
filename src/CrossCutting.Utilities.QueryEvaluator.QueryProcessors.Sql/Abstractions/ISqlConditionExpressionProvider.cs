namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlConditionExpressionProvider
{
    Result GetConditionExpression(IQuery query,
                                  object? context,
                                  ICondition condition,
                                  IQueryFieldInfo fieldInfo,
                                  ISqlExpressionProvider sqlExpressionProvider,
                                  ParameterBag parameterBag,
                                  Func<string, PagedSelectCommandBuilder> actionDelegate);
}
