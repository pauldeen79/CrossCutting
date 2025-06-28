namespace CrossCutting.Utilities.QueryEvaluator.Conditions;

public partial record ComposableCondition
{
    public async override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => await EvaluateTypedAsync(context, token).ConfigureAwait(false);

    public override Task<Result<bool>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => Task.Run(() => Operator.Evaluate(LeftExpression.Evaluate(context).Value, RightExpression.Evaluate(context).Value), token);
}
