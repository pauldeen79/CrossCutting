namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IExpression
{
    Result<object?> Evaluate();
    ExpressionParseResult Parse();
}

public interface IExpression<T> : IExpression
{
    Result<T> EvaluateTyped();
}
