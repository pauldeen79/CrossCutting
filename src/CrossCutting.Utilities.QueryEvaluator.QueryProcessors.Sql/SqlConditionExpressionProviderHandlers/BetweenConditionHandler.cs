namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class BetweenConditionHandler : ConditionExpressionHandlerBase<BetweenCondition>
{
    protected override async Task<Result> DoGetConditionExpressionAsync(StringBuilder builder, IQueryContext context, BetweenCondition condition, IQueryFieldInfo fieldInfo, ISqlExpressionProvider sqlExpressionProvider, ParameterBag parameterBag)
    {
        sqlExpressionProvider = ArgumentGuard.IsNotNull(sqlExpressionProvider, nameof(sqlExpressionProvider));
        condition = ArgumentGuard.IsNotNull(condition, nameof(condition));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(condition.SourceExpression), () => sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.SourceExpression), fieldInfo, parameterBag))
            .Add(nameof(condition.LowerBoundExpression), () => sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.LowerBoundExpression), fieldInfo, parameterBag))
            .Add(nameof(condition.UpperBoundExpression), () => sqlExpressionProvider.GetSqlExpressionAsync(context, new SqlExpression(condition.UpperBoundExpression), fieldInfo, parameterBag))
            .Build().ConfigureAwait(false))
            .OnSuccess(results => builder.Append($"{results.GetValue(nameof(condition.SourceExpression))} BETWEEN {results.GetValue(nameof(condition.LowerBoundExpression))} AND {results.GetValue(nameof(condition.UpperBoundExpression))}"));
    }
}
