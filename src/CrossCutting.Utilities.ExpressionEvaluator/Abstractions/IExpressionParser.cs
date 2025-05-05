namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionParser
{
    Result<IExpression> Parse(ICollection<ExpressionToken> tokens);
}
