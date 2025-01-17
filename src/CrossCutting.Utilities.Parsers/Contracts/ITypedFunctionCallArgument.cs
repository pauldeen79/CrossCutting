namespace CrossCutting.Utilities.Parsers.Contracts;

public interface ITypedFunctionCallArgument<T>
{
    Result<T> GetTypedValueResult(FunctionCallContext context);
}
