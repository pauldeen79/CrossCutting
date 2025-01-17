namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class TypedConstantArgumentBuilder<T> : TypedFunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new ConstantArgumentBuilder().WithValue(Value);

    public override TypedFunctionCallArgument<T> BuildTyped()
        => new TypedConstantArgument<T>(Value);

    public override FunctionCallArgument Build()
        => new TypedConstantArgument<T>(Value);

    public T Value { get; set; } = default!;

    public TypedConstantArgumentBuilder<T> WithValue(T value)
    {
        Value = value;
        return this;
    }
}
