namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record SmallerThanCondition
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new SmallerOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateAsync(context, token);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new SmallerOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateTypedAsync(context, token);
}
