namespace CrossCutting.Utilities.ExpressionEvaluator;

internal interface IOperator
{
    Result<object?> Evaluate(ExpressionEvaluatorContext context);
    ExpressionParseResult Parse(ExpressionEvaluatorContext context);
}
