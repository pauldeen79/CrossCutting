namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariableProcessor
{
    Result<object?> Process(string variableExpression, object? context);
}
