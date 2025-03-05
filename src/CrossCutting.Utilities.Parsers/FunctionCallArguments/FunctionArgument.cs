namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record FunctionArgument
{
    public override bool IsDynamic => true;

    public override Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.FunctionEvaluator.Evaluate(Function, context.Settings, context.Context);
    }

    public override Result<Type> Validate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.FunctionEvaluator.Validate(Function, context.Settings, context.Context);
    }
}
