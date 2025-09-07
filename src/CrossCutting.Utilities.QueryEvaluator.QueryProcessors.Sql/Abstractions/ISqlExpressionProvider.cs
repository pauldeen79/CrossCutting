namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProvider
{
    Result<string> GetSqlExpression(IQueryContext context, IExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag);
}
