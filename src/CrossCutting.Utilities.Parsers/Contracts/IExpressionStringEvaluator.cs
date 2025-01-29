namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionStringEvaluator
{
    Result<Type> Validate(string expressionString, ExpressionStringEvaluatorSettings settings, object? context, IFormattableStringParser? formattableStringParser);

    Result<object?> Evaluate(string expressionString, ExpressionStringEvaluatorSettings settings, object? context, IFormattableStringParser? formattableStringParser);
}
