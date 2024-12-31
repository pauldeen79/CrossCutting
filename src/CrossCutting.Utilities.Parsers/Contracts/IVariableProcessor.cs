namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariableProcessor
{
    Result Validate(string variableExpression, object? context);

    Result<object?> Evaluate(string variableExpression, object? context);
}
