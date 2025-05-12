namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IExpression
{
    Task<Result<object?>> EvaluateAsync();
    Task<ExpressionParseResult> ParseAsync();
}

public interface IExpression<T> : IExpression
{
    Task<Result<T>> EvaluateTypedAsync();
}
