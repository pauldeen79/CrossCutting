namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpression
{
    int Order { get; }

    Result Validate(string expression, IFormatProvider formatProvider, object? context);

    Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context);
}
