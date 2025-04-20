namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IExpressionTokenizer
{
    Result<List<ExpressionToken>> Tokenize(ExpressionEvaluatorContext context);
}
