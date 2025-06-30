namespace CrossCutting.Utilities.QueryEvaluator.Conditions;

public partial record SmallerThanOrEqualCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(FirstExpression), FirstExpression.EvaluateAsync(context, token))
            .Add(nameof(SecondExpression), SecondExpression.EvaluateAsync(context, token))
            .Build()
            .ConfigureAwait(false))
            .OnSuccess(results => SmallerOrEqualThan.Evaluate(results.GetValue(nameof(FirstExpression)), results.GetValue(nameof(SecondExpression))));
}
