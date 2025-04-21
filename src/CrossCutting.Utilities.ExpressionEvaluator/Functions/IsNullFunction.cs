namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("IsNull")]
[FunctionArgument("Expression", typeof(object))]
public class IsNullFunction : IFunction<bool>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<bool> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetArgumentValueResult(0, "Expression")
            .Transform(result => Result.Success(result.Value is null));
    }
}
