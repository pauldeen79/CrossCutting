namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariable
{
    Result Validate(string variableExpression, object? context);

    Result<object?> Evaluate(string variableExpression, object? context);
}
