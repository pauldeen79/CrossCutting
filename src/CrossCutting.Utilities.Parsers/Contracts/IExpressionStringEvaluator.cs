namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionStringEvaluator
{
    Result Validate(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);

    Result<object?> Evaluate(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
