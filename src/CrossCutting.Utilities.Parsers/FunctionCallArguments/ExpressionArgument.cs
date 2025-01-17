namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ExpressionArgument
{
    public override Result<object?> GetValueResult(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = context.ExpressionEvaluator.Evaluate(Value, context.FormatProvider, context.Context);

        return result.Status == ResultStatus.NotSupported
            ? Result.Success<object?>(Value)
            : result;
    }
}
