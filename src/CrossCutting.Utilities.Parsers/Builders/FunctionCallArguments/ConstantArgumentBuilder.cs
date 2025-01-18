namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class ConstantArgumentBuilder<T> : FunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new ConstantArgumentBuilder().WithValue(Value);

    public override FunctionCallArgument<T> BuildTyped()
        => new ConstantArgument<T>(Value);

    public override FunctionCallArgument Build()
        => new ConstantArgument<T>(Value);

    public T Value { get; set; } = default!;

    public ConstantArgumentBuilder<T> WithValue(T value)
    {
        Value = value;
        return this;
    }
}
