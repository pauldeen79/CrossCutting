namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class ConstantResultArgumentBuilder<T> : FunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new ConstantResultArgumentBuilder().WithResult(Result.Transform<object?>(value => value));

    public override FunctionCallArgument<T> BuildTyped()
        => new ConstantResultArgument<T>(Result);

    public override FunctionCallArgument Build()
        => new ConstantResultArgument<T>(Result);

    public Result<T> Result { get; set; } = default!;

    public ConstantResultArgumentBuilder<T> WithResult(Result<T> result)
    {
        Result = result;
        return this;
    }
}
