namespace CrossCutting.Utilities.Parsers.Builders.FunctionCallArguments;

public partial class ConstantResultArgumentBuilder<T>
{
#pragma warning disable CA2225 // Operator overloads have named alternates
    public static implicit operator ConstantResultArgumentBuilder<T>(Result<T> result)
        => new ConstantResultArgumentBuilder<T> { Result = result };
#pragma warning restore CA2225 // Operator overloads have named alternates
}
