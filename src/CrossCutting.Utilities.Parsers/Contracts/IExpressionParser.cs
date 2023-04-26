namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionParser
{
    Result<object?> Parse(string value, IFormatProvider formatProvider, object? context);
}
