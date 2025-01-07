namespace CrossCutting.Utilities.Parsers.Contracts;

//TODO: Rename request to Context
public interface IFunction
{
    Result Validate(FunctionCallContext request);

    Result<object?> Evaluate(FunctionCallContext request);
}
