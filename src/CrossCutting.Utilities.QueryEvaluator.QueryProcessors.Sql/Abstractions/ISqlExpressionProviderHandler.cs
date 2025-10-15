namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProviderHandler
{
    Task<Result<string>> GetSqlExpressionAsync(IQueryContext context, ISqlExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback);
}
