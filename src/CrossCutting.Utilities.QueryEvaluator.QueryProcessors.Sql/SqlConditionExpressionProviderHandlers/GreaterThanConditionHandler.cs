namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class GreaterThanConditionHandler : ConditionExpressionHandlerBase<GreaterThanCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, object? context, GreaterThanCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => GetSimpleConditionExpression(builder, query, context, condition, fieldInfo, sqlExpressionProvider, parameterBag, new ConditionParameters(">"));
}
