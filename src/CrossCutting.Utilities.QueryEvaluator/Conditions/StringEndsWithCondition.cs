namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record StringEndsWithCondition : IChildEvaluatablesContainer
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new StringEndsWithOperatorEvaluatable(StringComparison, SourceExpression, CompareExpression).EvaluateAsync(context, token);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new StringEndsWithOperatorEvaluatable(StringComparison, SourceExpression, CompareExpression).EvaluateTypedAsync(context, token);

    public IEnumerable<IEvaluatable> GetChildEvaluatables() =>
    [
        SourceExpression,
        CompareExpression
    ];

    public override IEvaluatableBuilder<bool> ToTypedBuilder()
        => ToTypedBuilderCore();
}
