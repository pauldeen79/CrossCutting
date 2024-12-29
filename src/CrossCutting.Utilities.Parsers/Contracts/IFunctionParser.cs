namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParser
{
    Result Validate (string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);

    Result<FunctionCall> Parse(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
