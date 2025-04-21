namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("IsNull")]
[FunctionArgument("Expression", typeof(object))]
public class IsNullFunction : IFunction<bool>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<bool> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add("Expression", () => context.GetArgumentValueResult(0, "Expression"))
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue("Expression") is null));
}
