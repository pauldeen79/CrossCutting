namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionComponent
{
    int Order { get; }
    ExpressionParseResult Parse(ExpressionEvaluatorContext context);
    Result<object?> Evaluate(ExpressionEvaluatorContext context);
}
