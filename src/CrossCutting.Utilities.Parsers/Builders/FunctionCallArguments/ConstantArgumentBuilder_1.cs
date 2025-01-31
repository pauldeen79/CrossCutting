namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class ConstantArgumentBuilder<T>
{
#pragma warning disable CA2225 // Operator overloads have named alternates
    public static implicit operator ConstantArgumentBuilder<T>(T value)
        => new ConstantArgumentBuilder<T> { Value = value };
#pragma warning restore CA2225 // Operator overloads have named alternates
}
