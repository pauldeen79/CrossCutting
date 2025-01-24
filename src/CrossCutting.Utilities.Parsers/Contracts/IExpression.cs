namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IExpression
{
    Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context);

    Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context);
}
