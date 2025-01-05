namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunction
{
    Result Validate(FunctionCallContext request);

    Result<object?> Evaluate(FunctionCallContext request);
}
