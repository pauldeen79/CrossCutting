namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IEvaluatable
{
    Task<Result<object?>> EvaluateAsync();
}

public interface IEvaluatable<T> : IEvaluatable
{
    Task<Result<T>> EvaluateTypedAsync();
}

public interface IExpression : IEvaluatable
{
    Task<ExpressionParseResult> ParseAsync();
}

public interface IExpression<T> : IExpression, IEvaluatable<T>
{
}
