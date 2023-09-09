namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParser
{
    Result<FunctionParseResult> Parse(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
