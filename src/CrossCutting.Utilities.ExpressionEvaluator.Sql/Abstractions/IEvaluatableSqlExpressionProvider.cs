namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface IEvaluatableSqlExpressionProvider
{
    Task<Result> GetConditionExpressionAsync(StringBuilder builder,
                                             object? context,
                                             IEvaluatable<bool> condition,
                                             IFieldNameProvider fieldNameProvider,
                                             //ISqlExpressionProvider sqlExpressionProvider,
                                             ParameterBag parameterBag,
                                             CancellationToken token);
}
