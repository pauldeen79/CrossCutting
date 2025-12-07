namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record GreaterThanCondition
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new GreaterOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateAsync(context, token);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new GreaterOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateTypedAsync(context, token);
}
