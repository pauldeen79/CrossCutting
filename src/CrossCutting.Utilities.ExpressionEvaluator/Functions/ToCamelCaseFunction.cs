namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("ToCamelCase")]
[FunctionArgument("StringExpression", typeof(object))]
public class ToCamelCaseFunction : IFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add("StringExpression", () => context.GetArgumentValueResult<string>(0, "StringExpression"))
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<string>("StringExpression").ToCamelCase(context.Context.Settings.FormatProvider.ToCultureInfo())));
}
