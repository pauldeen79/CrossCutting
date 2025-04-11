namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IOperatorExpressionTokenizer
{
    Result<List<OperatorExpressionToken>> Tokenize(string input);
}
