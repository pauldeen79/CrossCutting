namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IExpression
{
    Result<object?> Evaluate(ExpressionEvaluatorContext context);
    ExpressionParseResult Parse(ExpressionEvaluatorContext context);
}

public interface IExpression<T> : IExpression
{
    Result<T> EvaluateTyped(ExpressionEvaluatorContext context);
}
