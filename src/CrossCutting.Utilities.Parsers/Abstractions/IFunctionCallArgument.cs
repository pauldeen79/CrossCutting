namespace CrossCutting.Utilities.Parsers.Abstractions;

public partial interface IFunctionCallArgument
{
    Result<object?> Evaluate(FunctionCallContext context);
    Result<Type> Validate(FunctionCallContext context);
}
