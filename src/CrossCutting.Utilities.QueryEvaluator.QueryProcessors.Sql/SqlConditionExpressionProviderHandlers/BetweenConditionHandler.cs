namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class BetweenConditionHandler : ConditionExpressionHandlerBase<BetweenCondition>
{
    protected override Result DoGetConditionExpression(StringBuilder builder, IQueryContext context, BetweenCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
        => new ResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .Add(nameof(condition.LowerBoundExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(condition.LowerBoundExpression), fieldInfo, parameterBag))
            .Add(nameof(condition.UpperBoundExpression), () => sqlExpressionProvider.GetSqlExpression(context, new SqlExpression(condition.UpperBoundExpression), fieldInfo, parameterBag))
            .Build()
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} BETWEEN {results.GetValue(nameof(condition.LowerBoundExpression))} AND {results.GetValue(nameof(condition.UpperBoundExpression))}"));
}
