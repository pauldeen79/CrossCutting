namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("ToPascalCase")]
[FunctionArgument("StringExpression", typeof(object))]
public class ToPascalCaseFunction : IFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add("StringExpression", () => context.GetArgumentValueResult<string>(0, "StringExpression"))
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<string>("StringExpression").ToPascalCase(context.Context.Settings.FormatProvider.ToCultureInfo())));
}
