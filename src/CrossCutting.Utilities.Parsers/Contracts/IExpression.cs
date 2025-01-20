namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpression
{
    int Order { get; }

    Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context);

    Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context);
}
