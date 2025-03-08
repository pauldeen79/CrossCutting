namespace CrossCutting.Utilities.Parsers.FunctionCallTypeArguments;

public partial record FunctionTypeArgument
{
    public override bool IsDynamic => true;

    public override Result<Type> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.FunctionEvaluator.Evaluate(Function, context.Settings, context.Context).TryCast<Type>();
    }

    public override Result<Type> Validate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.FunctionEvaluator.Validate(Function, context.Settings, context.Context);
    }
}
