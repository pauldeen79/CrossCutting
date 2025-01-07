namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record DynamicArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.FunctionEvaluator.Evaluate(Function, context.ExpressionEvaluator, context.FormatProvider, context.Context);
    }
}
