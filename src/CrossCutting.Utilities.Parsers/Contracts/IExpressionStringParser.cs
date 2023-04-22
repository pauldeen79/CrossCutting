namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpressionStringParser
{
    Result<object> Parse(string input, IFormatProvider formatProvider);
}
