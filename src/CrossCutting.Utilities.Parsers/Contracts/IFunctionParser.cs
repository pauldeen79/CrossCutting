namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParser
{
    Result<FunctionCall> Parse(string function, IFormatProvider formatProvider, IFormattableStringParser? formattableStringParser, object? context);
}
