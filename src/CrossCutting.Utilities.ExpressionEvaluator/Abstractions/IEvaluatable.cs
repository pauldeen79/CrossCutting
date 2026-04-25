namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public partial interface IEvaluatable : IBuildableEntity<IEvaluatableBuilder>
{
    Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
public partial interface IEvaluatable<T> : IEvaluatable
{
    Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
