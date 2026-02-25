namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface IEvaluatableSqlExpressionProviderHandler
{
    Task<Result<string>> GetConditionExpressionAsync(object? context,
                                                     IEvaluatable evaluatable,
                                                     IFieldNameProvider fieldNameProvider,
                                                     ParameterBag parameterBag,
                                                     IEvaluatableSqlExpressionProviderHandler callback,
                                                     CancellationToken token);
}
