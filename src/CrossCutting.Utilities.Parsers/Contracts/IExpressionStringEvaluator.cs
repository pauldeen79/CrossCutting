namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionStringEvaluator
{
    Result Validate(string expressionString, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);

    Result<object?> Evaluate(string expressionString, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
