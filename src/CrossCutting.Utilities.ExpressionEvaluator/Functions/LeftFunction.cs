namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionArgument("StringExpression", typeof(string))]
[FunctionArgument("Length", typeof(int))]
public class LeftFunction : IFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add<string>(context, 0, "StringExpression")
            .Add<int>(context, 1, "Length")
            .Build()
            .OnSuccess(results =>
            {
                var stringExpression = results.GetValue<string>("StringExpression");
                var length = results.GetValue<int>("Length");

                return stringExpression.Length >= length
                    ? Result.Success(stringExpression.Substring(0, length))
                    : Result.Invalid<string>("Length must refer to a location within the string");
            });
    }
}
