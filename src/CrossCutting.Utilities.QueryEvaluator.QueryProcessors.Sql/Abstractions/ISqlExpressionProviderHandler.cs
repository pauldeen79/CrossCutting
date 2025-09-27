namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProviderHandler
{
    Result<string> GetSqlExpression(IQueryContext context, IEvaluatable expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag, ISqlExpressionProvider callback);
}
