namespace CrossCutting.Utilities.ExpressionEvaluator.Evaluatables;

public partial record PowerOperatorEvaluatable
{
    public override async Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add(nameof(LeftOperand), () => LeftOperand.EvaluateAsync(context, token))
            .Add(nameof(RightOperand), () => RightOperand.EvaluateAsync(context, token))
            .BuildAsync().ConfigureAwait(false))
            .OnSuccess(results => Power.Evaluate(results.GetValue(nameof(LeftOperand)), results.GetValue(nameof(RightOperand)), context.Settings.FormatProvider));
}
