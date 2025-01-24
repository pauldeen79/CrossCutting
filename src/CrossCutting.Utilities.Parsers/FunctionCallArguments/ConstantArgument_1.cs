namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantArgument<T> : FunctionCallArgument<T>
{
    public override Result<T> EvaluateTyped(FunctionCallContext context)
        => Result.Success(Value);

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Success<object?>(Value);

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(Value?.GetType()!);

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
