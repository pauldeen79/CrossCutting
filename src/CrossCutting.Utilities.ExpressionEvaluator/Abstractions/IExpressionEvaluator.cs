namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionEvaluator
{
    Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context);
    Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context);

    Task<ExpressionParseResult> ParseCallbackAsync(ExpressionEvaluatorContext context);
    Task<Result<object?>> EvaluateCallbackAsync(ExpressionEvaluatorContext context);
}
