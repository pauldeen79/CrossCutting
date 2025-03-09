namespace CrossCutting.Utilities.Parsers.FunctionCallTypeArguments;

public partial record DelegateResultTypeArgument
{
    public override bool IsDynamic => true;

    public override Result<Type> Evaluate(FunctionCallContext context)
        => Delegate();

    public override Result<Type> Validate(FunctionCallContext context)
        => ValidationDelegate is null
            ? Result.Success(typeof(Type))
            : ValidationDelegate.Invoke();
}
