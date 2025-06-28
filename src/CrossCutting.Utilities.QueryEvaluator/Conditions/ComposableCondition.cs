namespace CrossCutting.Utilities.QueryEvaluator.Conditions;

public partial record ComposableCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(LeftExpression), LeftExpression.EvaluateAsync(context, token))
            .Add(nameof(RightExpression), RightExpression.EvaluateAsync(context, token))
            .Build()
            .ConfigureAwait(false))
            .OnSuccess(results => Operator.Evaluate(results.GetValue(nameof(LeftExpression)), results.GetValue(nameof(RightExpression))));
}
