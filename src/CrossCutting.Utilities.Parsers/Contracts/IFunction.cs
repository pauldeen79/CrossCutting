namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunction
{
    Result<object?> Evaluate(FunctionCallContext context);
}
