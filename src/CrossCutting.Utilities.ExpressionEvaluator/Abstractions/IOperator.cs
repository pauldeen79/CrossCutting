namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IOperator
{
    Result<object?> Evaluate(ExpressionEvaluatorContext context);
    Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context);
    ExpressionParseResult Parse(ExpressionEvaluatorContext context);
}
