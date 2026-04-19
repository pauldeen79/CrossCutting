namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlConditionExpressionProviderHandler
{
    Task<Result> GetConditionExpressionAsync(StringBuilder builder,
                                             object? context,
                                             IEvaluatable<bool> condition,
                                             IEntityFieldInfo fieldInfo,
                                             ISqlExpressionProvider sqlExpressionProvider,
                                             ParameterBag parameterBag,
                                             CancellationToken token);
}
