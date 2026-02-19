namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProvider
{
    Task<Result<string>> GetSqlExpressionAsync(object? context, ISqlExpression expression, IFieldNameProvider fieldInfo, ParameterBag parameterBag, CancellationToken token);
}
