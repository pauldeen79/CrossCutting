namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record GreaterThanOrEqualCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => ConditionHelper.EvaluateObjectConditionAsync(SourceExpression, CompareExpression, context, (first, second) => GreaterOrEqualThan.Evaluate(first, second), token);
}
