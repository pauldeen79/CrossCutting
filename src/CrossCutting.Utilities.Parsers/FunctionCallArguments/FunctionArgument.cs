namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record FunctionArgument
{
    public override Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.FunctionEvaluator.Evaluate(Function, context.FormatProvider, context.Context);
    }

    public override Result<Type> Validate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.FunctionEvaluator.Validate(Function, context.FormatProvider, context.Context);
    }
}
