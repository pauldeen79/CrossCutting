namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class TypedConstantArgumentBuilder<T> : ITypedFunctionCallArgumentBuilder<T>
{
    public FunctionCallArgumentBuilder ToUntyped()
        => new ConstantArgumentBuilder().WithValue(Value);

    ITypedFunctionCallArgument<T> ITypedFunctionCallArgumentBuilder<T>.Build()
        => new TypedConstantArgument<T>(Value);
}
