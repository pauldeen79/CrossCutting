namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record TypedConstantArgument<T> : TypedFunctionCallArgument<T>
{
    public Result<T> GetTypedValueResult(FunctionCallContext context)
        => Result.Success(Value);

    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result.Success<object?>(Value);

    public override FunctionCallArgumentBuilder ToBuilder()
        => new TypedConstantArgumentBuilder<T>().WithValue(Value);

    public override TypedFunctionCallArgumentBuilder<T> ToTypedBuilder()
        => new TypedConstantArgumentBuilder<T>().WithValue(Value);

    public T Value { get; }

    public TypedConstantArgument(T value)
    {
        Value = value;
    }
}
