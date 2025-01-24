namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IMathematicExpressionEvaluator
{
    Result<Type> Validate(string expression, IFormatProvider formatProvider, object? context);

    Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context);
}
