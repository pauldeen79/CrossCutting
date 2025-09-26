namespace CrossCutting.Utilities.ExpressionEvaluator;

public abstract partial record ExpressionBase
{
    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
    public abstract Task<ExpressionParseResult> ParseAsync(CancellationToken token);

    IExpressionBuilder IBuildableEntity<IExpressionBuilder>.ToBuilder() => ToBuilder();
}
