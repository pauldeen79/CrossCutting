namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProvider
{
    Result<string> GetSqlExpression(IQuery query, IExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag);
}
