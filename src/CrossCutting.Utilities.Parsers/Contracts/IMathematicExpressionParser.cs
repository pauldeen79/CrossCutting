namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IMathematicExpressionParser
{
    Result<object?> Parse(string input, IFormatProvider formatProvider, object? context);
}
