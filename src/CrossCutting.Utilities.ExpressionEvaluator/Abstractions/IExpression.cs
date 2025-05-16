namespace CrossCutting.Utilities.ExpressionEvaluator;

public interface IExpression : IEvaluatable
{
    Task<ExpressionParseResult> ParseAsync();
}

public interface IExpression<T> : IExpression, IEvaluatable<T>
{
}
