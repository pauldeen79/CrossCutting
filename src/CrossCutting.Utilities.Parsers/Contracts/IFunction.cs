namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunction
{
    Result Validate(FunctionCallRequest request);

    Result<object?> Evaluate(FunctionCallRequest request);
}
