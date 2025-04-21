namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("ToLowerCase")]
[FunctionArgument("StringExpression", typeof(object))]
public class ToLowerCaseFunction : IFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add("StringExpression", () => context.GetArgumentValueResult<string>(0, "StringExpression"))
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<string>("StringExpression").ToLower(context.Context.Settings.FormatProvider.ToCultureInfo())));
}
