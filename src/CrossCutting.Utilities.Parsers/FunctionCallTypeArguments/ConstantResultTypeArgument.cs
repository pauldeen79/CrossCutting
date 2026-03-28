namespace CrossCutting.Utilities.Parsers.FunctionCallTypeArguments;

public partial record ConstantResultTypeArgument
{
    public override bool IsDynamic => false;

    public override Result<Type> Evaluate(FunctionCallContext context)
        => Value;

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(typeof(Type));
}
