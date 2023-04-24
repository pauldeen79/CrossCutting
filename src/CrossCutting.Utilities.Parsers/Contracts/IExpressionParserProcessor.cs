namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionParserProcessor
{
    int Order { get; }
    Result<object?> Parse(string value, IFormatProvider formatProvider);
}
