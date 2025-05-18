namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionEvaluator
{
    Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token);
    Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);

    Task<ExpressionParseResult> ParseCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token);
    Task<Result<object?>> EvaluateCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
