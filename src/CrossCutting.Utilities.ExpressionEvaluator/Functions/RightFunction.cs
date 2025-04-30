namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(string))]
[FunctionArgument("StringExpression", typeof(string))]
[FunctionArgument("Length", typeof(int))]
public class RightFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
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
                    ? Result.Success<object?>(stringExpression.Substring(stringExpression.Length - length, length))
                    : Result.Invalid<object?>("Length must refer to a location within the string");
            });
    }
}
