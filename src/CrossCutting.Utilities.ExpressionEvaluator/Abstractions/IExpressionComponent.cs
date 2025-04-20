namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionComponent
{
    int Order { get; }

    ExpressionParseResult Parse(ExpressionEvaluatorContext context);

    Result<object?> Evaluate(ExpressionEvaluatorContext context);
}

public interface IExpressionComponent<T> : IExpressionComponent
{
    Result<T> EvaluateTyped(ExpressionEvaluatorContext context);
}
