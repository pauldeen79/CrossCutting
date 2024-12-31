namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IMathematicExpressionEvaluator
{
    Result Validate(string input, IFormatProvider formatProvider, object? context);

    Result<object?> Evaluate(string input, IFormatProvider formatProvider, object? context);
}
