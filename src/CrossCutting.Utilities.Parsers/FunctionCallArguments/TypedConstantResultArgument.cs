namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record TypedConstantResultArgument<T> : TypedFunctionCallArgument<T>
{
    public Result<T> GetTypedValueResult(FunctionCallContext context)
        => Result;

    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result.Transform<object?>(value => value);

    public override FunctionCallArgumentBuilder ToBuilder()
        => new TypedConstantResultArgumentBuilder<T>().WithResult(Result);

    public override TypedFunctionCallArgumentBuilder<T> ToTypedBuilder()
        => new TypedConstantResultArgumentBuilder<T>().WithResult(Result);

    public Result<T> Result { get; }

    public TypedConstantResultArgument(Result<T> result)
    {
        Result = result;
    }
}
