namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("ToString")]
[FunctionArgument("Expression", typeof(object))]
public class ToStringFunction : IFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add("Expression", () => context.GetArgumentValueResult(0, "Expression"))
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue("Expression").ToStringWithDefault()));
}
