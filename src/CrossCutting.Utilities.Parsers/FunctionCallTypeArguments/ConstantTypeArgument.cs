namespace CrossCutting.Utilities.Parsers.FunctionCallTypeArguments;

public partial record ConstantTypeArgument
{
    public override bool IsDynamic => false;

    public override Result<Type> Evaluate(FunctionCallContext context)
        => Result.Success(Value);

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(typeof(Type));
}
