namespace CrossCutting.Utilities.QueryEvaluator.QueryProcessors.Sql.SqlConditionExpressionProviders;

public class NullConditionHandler : ConditionExpressionHandlerBase<NullCondition>
{
    protected override async Task<Result> DoGetConditionExpressionAsync(ConditionExpressionHandlerContext<NullCondition> context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder<string>()
            .Add(nameof(context.Condition.SourceExpression), () => context.SqlExpressionProvider.GetSqlExpressionAsync(context.Context, new SqlExpression(context.Condition.SourceExpression), context.FieldInfo, context.ParameterBag, token))
            .BuildAsync(token).ConfigureAwait(false))
            .OnSuccess(results => context.Builder.Append($"{results.GetValue(nameof(context.Condition.SourceExpression))} IS NULL"));
    }
}
