namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface IEvaluatableSqlExpressionProvider
{
    Task<Result> GetConditionExpressionAsync(SelectCommandBuilder selectCommandBuilder,
                                             object? context,
                                             IEvaluatable<bool> condition,
                                             IFieldNameProvider fieldNameProvider,
                                             ParameterBag parameterBag,
                                             CancellationToken token);
}
