namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariable
{
    Result Validate(string expression, object? context);

    Result<object?> Evaluate(string expression, object? context);
}
