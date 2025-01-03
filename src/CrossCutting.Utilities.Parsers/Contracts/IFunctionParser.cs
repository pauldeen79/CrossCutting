namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunctionParser
{
    Result<FunctionCall> Parse(string function, IFormatProvider formatProvider, object? context, IFormattableStringParser? formattableStringParser);
}
