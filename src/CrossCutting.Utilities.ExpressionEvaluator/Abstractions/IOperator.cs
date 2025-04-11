namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IOperator
{
    Result<object?> Evaluate(ExpressionEvaluatorContext context);
    ExpressionParseResult Parse(ExpressionEvaluatorContext context);
}
