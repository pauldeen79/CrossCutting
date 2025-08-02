namespace CrossCutting.Utilities.QueryEvaluator.Core.Conditions;

public partial record NotInCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => ConditionHelper.EvaluateObjectConditionAsync(FirstExpression, SecondExpression, context, (first, second) => NotIn.Evaluate(first, second, context.Settings.StringComparison), token);
}
