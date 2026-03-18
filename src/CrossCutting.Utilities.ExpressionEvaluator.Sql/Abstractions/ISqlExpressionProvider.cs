namespace CrossCutting.Utilities.ExpressionEvaluator.Sql.Abstractions;

public interface ISqlExpressionProvider
{
    Task<Result<string>> GetSqlExpressionAsync(object? context, ISqlExpression expression, IFieldNameProvider fieldNameProvider, ParameterBag parameterBag, CancellationToken token);
}
