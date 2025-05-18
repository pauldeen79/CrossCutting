namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

//TODO: Split into evaluator and evaluatorcallback, so it can be implemented explicitly
public interface IExpressionEvaluator
{
    Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context, CancellationToken token);
    Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token);

    Task<ExpressionParseResult> ParseCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token);
    Task<Result<object?>> EvaluateCallbackAsync(ExpressionEvaluatorContext context, CancellationToken token);
}
