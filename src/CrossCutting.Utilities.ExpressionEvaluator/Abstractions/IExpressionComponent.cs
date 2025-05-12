namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionComponent
{
    int Order { get; }
    Task<ExpressionParseResult> ParseAsync(ExpressionEvaluatorContext context);
    Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context);
}
