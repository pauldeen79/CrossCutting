namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IMathematicExpressionParser
{
    Result Validate(string input, IFormatProvider formatProvider, object? context);

    Result<object?> Parse(string input, IFormatProvider formatProvider, object? context);
}
