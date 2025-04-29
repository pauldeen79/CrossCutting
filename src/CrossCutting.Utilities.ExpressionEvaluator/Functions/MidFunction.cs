namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionArgument("StringExpression", typeof(string))]
[FunctionArgument("Index", typeof(int))]
[FunctionArgument("Length", typeof(int))]
public class MidFunction : IFunction<string>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<string> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add<string>(context, 0, "StringExpression")
            .Add<int>(context, 1, "Index")
            .Add<int>(context, 2, "Length")
            .Build()
            .OnSuccess(results =>
            {
                var stringExpression = results.GetValue<string>("StringExpression");
                var index = results.GetValue<int>("Index");
                var length = results.GetValue<int>("Length");

                return stringExpression.Length >= index + length
                    ? Result.Success(stringExpression.Substring(index, length))
                    : Result.Invalid<string>("Index and length must refer to a location within the string");
            });
    }
}
