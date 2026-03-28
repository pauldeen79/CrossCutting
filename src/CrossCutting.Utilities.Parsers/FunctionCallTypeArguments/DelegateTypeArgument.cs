namespace CrossCutting.Utilities.Parsers.FunctionCallTypeArguments;

public partial record DelegateTypeArgument
{
    public override bool IsDynamic => true;

    public override Result<Type> Evaluate(FunctionCallContext context)
        => Delegate();

    public override Result<Type> Validate(FunctionCallContext context)
        => ValidationDelegate is null
            ? typeof(Type)
            : ValidationDelegate.Invoke()!;
}
