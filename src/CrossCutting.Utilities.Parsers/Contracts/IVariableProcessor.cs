namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariableProcessor
{
    Result Validate(string expression, object? context);

    Result<object?> Evaluate(string expression, object? context);
}
