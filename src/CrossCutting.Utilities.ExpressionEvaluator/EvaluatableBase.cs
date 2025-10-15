namespace CrossCutting.Utilities.ExpressionEvaluator;

public abstract partial record EvaluatableBase
{
    public abstract Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);

    IEvaluatableBuilder IBuildableEntity<IEvaluatableBuilder>.ToBuilder() => ToBuilder();
}
