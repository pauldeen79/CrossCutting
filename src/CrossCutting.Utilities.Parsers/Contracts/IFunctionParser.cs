namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParser
{
    Result<FunctionCall> Parse(string input, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
