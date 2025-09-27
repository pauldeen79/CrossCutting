namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpression : IEvaluatable
{
    Task<ExpressionParseResult> ParseAsync(CancellationToken token);
}

public interface IExpression<T> : IExpression, IEvaluatable<T>
{
}
