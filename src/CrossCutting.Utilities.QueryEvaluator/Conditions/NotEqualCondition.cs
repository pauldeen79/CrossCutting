namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record NotEqualCondition
{
    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new NotEqualOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateAsync(context, token);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => new NotEqualOperatorEvaluatable(SourceExpression, CompareExpression).EvaluateTypedAsync(context, token);
}
