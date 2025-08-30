namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class BetweenConditionHandler : ConditionExpressionHandlerBase<BetweenCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQuery query, BetweenCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.SourceExpression, fieldInfo, parameterBag))
            .Add(nameof(condition.LowerBoundExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.LowerBoundExpression, fieldInfo, parameterBag))
            .Add(nameof(condition.UpperBoundExpression), () => sqlExpressionProvider.GetSqlExpression(query, condition.UpperBoundExpression, fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} BETWEEN {results.GetValue(nameof(condition.LowerBoundExpression))} AND {results.GetValue(nameof(condition.UpperBoundExpression))}"));
}
