namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionParserProcessor
{
    int Order { get; }

    Result Validate(string value, IFormatProvider formatProvider, object? context);

    Result<object?> Parse(string value, IFormatProvider formatProvider, object? context);
}
