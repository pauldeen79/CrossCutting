namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProvider
{
    Task<Result<string>> GetSqlExpressionAsync(IQueryContext context, ISqlExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag);
}
