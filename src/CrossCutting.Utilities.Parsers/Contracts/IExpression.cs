namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpression
{
    int Order { get; }

    Result Validate(string value, IFormatProvider formatProvider, object? context);

    Result<object?> Evaluate(string value, IFormatProvider formatProvider, object? context);
}
