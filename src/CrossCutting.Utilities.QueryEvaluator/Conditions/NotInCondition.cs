namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record NotInCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(SourceExpression), () => SourceExpression.EvaluateAsync(context, token))
            .AddRange($"{nameof(CompareExpressions)}.{{0}}", CompareExpressions.Select(x => new Func<Task<Result<object?>>>(() => x.EvaluateAsync(context, token))))
            .BuildAsync(token)
            .ConfigureAwait(false))
            .OnSuccess(results => Result.Success(!results.GetValue(nameof(SourceExpression)).In(context.Settings.StringComparison, results.Where(x => x.Key.StartsWith($"{nameof(CompareExpressions)}.")).Select(x => x.Value.GetValue()))));
}
