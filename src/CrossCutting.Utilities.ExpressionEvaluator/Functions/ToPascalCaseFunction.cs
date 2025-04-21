namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("ToPascalCase")]
[FunctionArgument("StringExpression", typeof(object))]
public class ToPascalCaseFunction : IFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetArgumentValueResult<string>(0, "StringExpression")
            .Transform(result => Result.Success(result.GetValueOrThrow().ToPascalCase(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
