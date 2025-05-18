namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionComponent
{
    int Order { get; }
    Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token);
    Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);
}

public interface IExpressionComponent<T> : IExpressionComponent
{
    Task<Result<T>> EvaluateTypedAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
