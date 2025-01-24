namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ExpressionArgument
{
    public override Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = context.ExpressionEvaluator.Evaluate(Value, context.FormatProvider, context.Context);

        return result.Status == ResultStatus.NotSupported
            ? Result.Success<object?>(Value)
            : result;
    }

    public override Result<Type> Validate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = context.ExpressionEvaluator.Validate(Value, context.FormatProvider, context.Context);

        return result.Status == ResultStatus.Invalid && result.ErrorMessage?.StartsWith("Unknown expression type found in fragment:") == true
            ? Result.Continue(typeof(string))
            : result;
    }
}
