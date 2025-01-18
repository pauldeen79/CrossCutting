namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument<T> : FunctionCallArgument<T>
{
    public Result<T> GetTypedValueResult(FunctionCallContext context)
        => Result.Success(Value);

    public override Result<object?> GetValueResult(FunctionCallContext context)
        => Result.Success<object?>(Value);

    public override FunctionCallArgumentBuilder ToBuilder()
        => new ConstantArgumentBuilder<T>().WithValue(Value);

    public override FunctionCallArgumentBuilder<T> ToTypedBuilder()
        => new ConstantArgumentBuilder<T>().WithValue(Value);

    public T Value { get; }

    public ConstantArgument(T value)
    {
        Value = value;
    }
}
