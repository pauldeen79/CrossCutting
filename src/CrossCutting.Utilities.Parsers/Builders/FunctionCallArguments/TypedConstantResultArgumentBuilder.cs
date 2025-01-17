namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class TypedConstantResultArgumentBuilder<T> : TypedFunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new ConstantResultArgumentBuilder().WithResult(Result.Transform<object?>(value => value));

    public override TypedFunctionCallArgument<T> BuildTyped()
        => new TypedConstantResultArgument<T>(Result);

    public override FunctionCallArgument Build()
        => new TypedConstantResultArgument<T>(Result);

    public Result<T> Result { get; set; } = default!;

    public TypedConstantResultArgumentBuilder<T> WithResult(Result<T> result)
    {
        Result = result;
        return this;
    }
}
