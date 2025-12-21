namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record GreaterThanOrEqualCondition
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new GreaterOrEqualOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateAsync(context, token);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new GreaterOrEqualOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateTypedAsync(context, token);
}
