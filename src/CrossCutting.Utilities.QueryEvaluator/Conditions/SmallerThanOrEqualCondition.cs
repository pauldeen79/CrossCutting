namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record SmallerThanOrEqualCondition
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new SmallerOrEqualOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateAsync(context, token);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new SmallerOrEqualOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateTypedAsync(context, token);
}
