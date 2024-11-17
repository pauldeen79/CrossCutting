namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariable
{
    Result<object?> Process(string variable, object? context);
}
