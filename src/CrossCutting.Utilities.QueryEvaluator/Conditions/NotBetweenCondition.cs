namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record NotBetweenCondition : IChildEvaluatablesContainer
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add(nameof(SourceExpression), () => SourceExpression.EvaluateAsync(context, token))
            .Add(nameof(LowerBoundExpression), () => LowerBoundExpression.EvaluateAsync(context, token))
            .Add(nameof(UpperBoundExpression), () => UpperBoundExpression.EvaluateAsync(context, token))
            .BuildAsync(token)
            .ConfigureAwait(false))
            .OnSuccess(results => NotBetween.Evaluate(results.GetValue(nameof(SourceExpression)), results.GetValue(nameof(LowerBoundExpression)), results.GetValue(nameof(UpperBoundExpression))));
    }

    public IEnumerable<IEvaluatable> GetChildEvaluatables() =>
    [
        SourceExpression,
        LowerBoundExpression,
        UpperBoundExpression
    ];
}
