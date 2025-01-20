namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ConstantResultArgument<T> : FunctionCallArgument<T>
{
    public Result<T> GetTypedValueResult(FunctionCallContext context)
        => Result;

    public override Result<object?> Evaluate(FunctionCallContext context)
        => Result.Transform<object?>(value => value);

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Transform(x => x?.GetType()!);

    public override FunctionCallArgumentBuilder ToBuilder()
        => new ConstantResultArgumentBuilder<T>().WithResult(Result);

    public override FunctionCallArgumentBuilder<T> ToTypedBuilder()
        => new ConstantResultArgumentBuilder<T>().WithResult(Result);

    public Result<T> Result { get; }

    public ConstantResultArgument(Result<T> result)
    {
        Result = result;
    }
}
