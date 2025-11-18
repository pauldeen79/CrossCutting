namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class BetweenConditionHandler : ConditionExpressionHandlerBase<BetweenCondition>
{
    protected override async Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<BetweenCondition> context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(context.Condition.SourceExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(context.Condition.SourceExpression), context.FieldInfo, context.ParameterBag, token))
            .Add(nameof(context.Condition.LowerBoundExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(context.Condition.LowerBoundExpression), context.FieldInfo, context.ParameterBag, token))
            .Add(nameof(context.Condition.UpperBoundExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(context.Condition.UpperBoundExpression), context.FieldInfo, context.ParameterBag, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results => context.Builder.Append($"{results.GetValue(nameof(context.Condition.SourceExpression))} BETWEEN {results.GetValue(nameof(context.Condition.LowerBoundExpression))} AND {results.GetValue(nameof(context.Condition.UpperBoundExpression))}"));
    }
}
