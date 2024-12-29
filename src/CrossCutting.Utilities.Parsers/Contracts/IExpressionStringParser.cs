namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionStringParser
{
    Result Validate(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);

    Result<object?> Parse(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
