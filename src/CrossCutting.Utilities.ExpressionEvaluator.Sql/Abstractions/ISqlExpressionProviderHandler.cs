namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface ISqlExpressionProviderHandler
{
    Task<Result<string>> GetSqlExpressionAsync(object? context, ISqlExpression expression, IFieldNameProvider fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback, CancellationToken token);
}
