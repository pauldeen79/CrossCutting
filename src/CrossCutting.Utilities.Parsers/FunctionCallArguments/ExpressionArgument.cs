namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record ExpressionArgument
{
    public override bool IsDynamic => false;

    public override Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = context.ExpressionEvaluator.Evaluate(Expression, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(context.Settings.FormatProvider), context.Context);

        return result.Status == ResultStatus.NotSupported
            ? Result.Success<object?>(Expression)
            : result;
    }

    public override Result<Type> Validate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = context.ExpressionEvaluator.Validate(Expression, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(context.Settings.FormatProvider), context.Context);

        return result.Status == ResultStatus.Invalid && result.ErrorMessage?.StartsWith("Unknown expression type found in fragment:") == true
            ? Result.Continue(typeof(string))
            : result;
    }
}
