namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariable
{
    Result Validate(string variableExpression, object? context);

    Result<object?> Process(string variableExpression, object? context);
}
