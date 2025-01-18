namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public abstract class FunctionCallArgumentBuilder<T> : FunctionCallArgumentBuilder
{
    public abstract FunctionCallArgument<T> BuildTyped();

#pragma warning disable CA2225 // Operator overloads have named alternates
    public static implicit operator FunctionCallArgumentBuilder<T>(T value)
#pragma warning restore CA2225 // Operator overloads have named alternates
        => new ConstantArgumentBuilder<T> { Value = value };
}
