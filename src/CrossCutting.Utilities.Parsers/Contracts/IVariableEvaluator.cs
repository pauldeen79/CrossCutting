namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariableEvaluator
{
    Result<Type> Validate(string expression, object? context);

    Result<object?> Evaluate(string expression, object? context);
}
