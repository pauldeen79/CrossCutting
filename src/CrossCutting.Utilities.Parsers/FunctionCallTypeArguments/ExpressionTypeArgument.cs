namespace CrossCutting.Utilities.Parsers.FunctionCallTypeArguments;

public partial record ExpressionTypeArgument
{
    public override bool IsDynamic => false;

    public override Result<Type> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = context.ExpressionEvaluator.Evaluate(Expression, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(context.Settings.FormatProvider), context.Context).TryCast<Type>();

        return result.Status == ResultStatus.NotSupported
            ? GetExpressionType()
            : result;
    }

    public override Result<Type> Validate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = context.ExpressionEvaluator.Validate(Expression, new ExpressionEvaluatorSettingsBuilder().WithFormatProvider(context.Settings.FormatProvider), context.Context);

        return result.Status == ResultStatus.Invalid && result.ErrorMessage?.StartsWith("Unknown expression type found in fragment:") == true
            ? Result.Continue<Type>()
            : result;
    }

    private Result<Type> GetExpressionType()
    {
        var type = Type.GetType(Expression, false);
        if (type is null)
        {
            return Result.Invalid<Type>($"Unknown type: {Expression}");
        }

        return Result.Success(type);
    }
}
