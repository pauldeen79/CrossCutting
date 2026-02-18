namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProviderHandler
{
    Task<Result<string>> GetSqlExpressionAsync(IContextContainer context, ISqlExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback, CancellationToken token);
}
