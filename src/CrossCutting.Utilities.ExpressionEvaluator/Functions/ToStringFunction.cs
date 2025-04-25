namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("ToString")]
[FunctionArgument("Expression", typeof(object))]
public class ToStringFunction : IFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetArgumentValueResult(0, "Expression")
            .Transform(result => Result.Success(result.Value.ToString(context.Context.Settings.FormatProvider)));
    }
}
