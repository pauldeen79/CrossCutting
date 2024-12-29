namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariableProcessor
{
    Result Validate(string variableExpression, object? context);

    Result<object?> Process(string variableExpression, object? context);
}
