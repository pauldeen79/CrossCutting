namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlConditionExpressionProvider
{
    Task<Result<string>> GetConditionExpressionAsync(object? context,
                                                     ICondition condition,
                                                     IEntityFieldInfo fieldInfo,
                                                     ISqlExpressionProvider sqlExpressionProvider,
                                                     ParameterBag parameterBag,
                                                     CancellationToken token);
}
