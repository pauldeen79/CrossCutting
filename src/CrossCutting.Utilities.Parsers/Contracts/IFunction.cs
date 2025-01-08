namespace CrossCutting.Utilities.Parsers.Contracts;

public interface IFunction
{
    Result Validate(FunctionCallContext context);

    Result<object?> Evaluate(FunctionCallContext context);
}
