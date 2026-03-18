namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record EqualCondition : IChildEvaluatablesContainer
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new EqualOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateAsync(context, token);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new EqualOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateTypedAsync(context, token);

    public IEnumerable<IEvaluatable> GetChildEvaluatables() =>
    [
        SourceExpression,
        CompareExpression
    ];
}
