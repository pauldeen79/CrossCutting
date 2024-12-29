namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionParser
{
    Result Validate(string value, IFormatProvider formatProvider, object? context);

    Result<object?> Parse(string value, IFormatProvider formatProvider, object? context);
}
