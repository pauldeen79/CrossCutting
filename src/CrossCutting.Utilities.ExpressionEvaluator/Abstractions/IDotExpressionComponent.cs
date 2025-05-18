namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IDotExpressionComponent
{
    int Order { get; }
    Task<Result<object?>> EvaluateAsync(DotExpressionComponentState state, CancellationToken token);
    Task<Result<Type>> ValidateAsync(DotExpressionComponentState state, CancellationToken token);
}
