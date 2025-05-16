namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(string))]
[MemberArgument("StringExpression", typeof(string))]
[MemberArgument("Index", typeof(int))]
[MemberArgument("Length", typeof(int))]
public class MidFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add<string>(context, 0, "StringExpression")
            .Add<int>(context, 1, "Index")
            .Add<int>(context, 2, "Length")
            .Build().ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var stringExpression = results.GetValue<string>("StringExpression");
                var index = results.GetValue<int>("Index");
                var length = results.GetValue<int>("Length");

                return stringExpression.Length >= index + length
                    ? Result.Success<object?>(stringExpression.Substring(index, length))
                    : Result.Invalid<object?>("Index and length must refer to a location within the string");
            });
    }
}
