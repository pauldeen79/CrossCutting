namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IVariableProcessor
{
    Result<object?> Process(string variable, object? context);
}
