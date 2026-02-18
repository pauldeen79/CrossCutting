namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlConditionExpressionProviderHandler
{
    Task<Result> GetConditionExpressionAsync(StringBuilder builder,
                                             IContextContainer context,
                                             IEvaluatable<bool> condition,
                                             IQueryFieldInfo fieldInfo,
                                             ISqlExpressionProvider sqlExpressionProvider,
                                             ParameterBag parameterBag,
                                             CancellationToken token);
}
