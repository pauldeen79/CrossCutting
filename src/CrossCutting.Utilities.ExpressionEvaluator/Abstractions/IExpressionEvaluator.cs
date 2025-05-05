namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionEvaluator
{
    ExpressionParseResult Parse(ExpressionEvaluatorContext context);
    Result<object?> Evaluate(ExpressionEvaluatorContext context);
    Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context);

    ExpressionParseResult ParseCallback(ExpressionEvaluatorContext context);
    Result<object?> EvaluateCallback(ExpressionEvaluatorContext context);
    Result<T> EvaluateTypedCallback<T>(ExpressionEvaluatorContext context);
}
