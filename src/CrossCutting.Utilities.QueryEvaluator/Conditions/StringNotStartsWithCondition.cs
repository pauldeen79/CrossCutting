namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record StringNotStartsWithCondition : IChildEvaluatablesContainer
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override async Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new StringStartsWithOperatorEvaluatable(StringComparison, SourceExpression, CompareExpression).EvaluateTypedAsync(context, token).ConfigureAwait(false)).Transform(x => !x);

    public IEnumerable<IEvaluatable> GetChildEvaluatables() =>
    [
        SourceExpression,
        CompareExpression
    ];
}
