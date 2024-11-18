namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariable
{
    Result<object?> Process(string variableExpression, object? context);
}
