namespace CrossCutting.Utilities.Parsers.Abstractions;

public partial interface ITypedFunctionCallArgument<T>
{
    Result<T> EvaluateTyped(FunctionCallContext context);
}
