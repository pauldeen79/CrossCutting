namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariable
{
    Result<Type> Validate(string expression, object? context);

    Result<object?> Evaluate(string expression, object? context);
}
