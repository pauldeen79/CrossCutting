namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProvider
{
    string GetSqlExpression(IQuery query, IExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag);
}
