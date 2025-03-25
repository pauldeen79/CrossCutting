namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionEvaluator
{
    Result<ExpressionParseResult> Parse(ExpressionEvaluatorContext context);

    Result<object?> Evaluate(ExpressionEvaluatorContext context);

    Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context);
}
