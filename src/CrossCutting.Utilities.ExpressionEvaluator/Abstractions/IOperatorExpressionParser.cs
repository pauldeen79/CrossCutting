namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IOperatorExpressionParser
{
    Result<IOperator> Parse(ICollection<OperatorExpressionToken> tokens);
}
