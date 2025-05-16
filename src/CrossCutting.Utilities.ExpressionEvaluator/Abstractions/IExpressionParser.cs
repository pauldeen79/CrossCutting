namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionParser
{
    Result<IExpression> Parse(ExpressionEvaluatorContext context, ICollection<ExpressionToken> tokens);
}
