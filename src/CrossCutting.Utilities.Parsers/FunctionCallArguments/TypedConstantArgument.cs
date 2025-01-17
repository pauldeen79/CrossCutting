namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record TypedConstantArgument<T> : ITypedFunctionCallArgument<T>
{
    public Result<T> GetTypedValueResult(FunctionCallContext context)
        => Result.Success(Value);

    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result.Success<object?>(Value);
}
