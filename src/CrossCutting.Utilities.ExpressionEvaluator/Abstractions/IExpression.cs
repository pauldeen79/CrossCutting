namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public partial interface IExpression : IEvaluatable, IBuildableEntity<IExpressionBuilder>
{
    Task<ExpressionParseResult> ParseAsync(CancellationToken token);
}

public interface IExpression<T> : IExpression, IEvaluatable<T>
{
}
