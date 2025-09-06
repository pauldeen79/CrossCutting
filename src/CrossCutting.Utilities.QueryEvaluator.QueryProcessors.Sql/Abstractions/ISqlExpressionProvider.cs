namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.Abstractions;

public interface ISqlExpressionProvider
{
    Result<string> GetSqlExpression(IQuery query, object? context, IExpression expression, IQueryFieldInfo fieldInfo, ParameterBag parameterBag);
}
