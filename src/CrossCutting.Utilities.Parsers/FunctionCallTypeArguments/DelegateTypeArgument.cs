namespace CrossCutting.Utilities.Parsers.FunctionCallTypeArguments;

public partial record DelegateTypeArgument
{
    public override bool IsDynamic => true;

    public override Result<Type> Evaluate(FunctionCallContext context)
        => Result.Success(Delegate());

    public override Result<Type> Validate(FunctionCallContext context)
        => Result.Success(ValidationDelegate?.Invoke()!);
}
