namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(string))]
[MemberArgument("StringExpression", typeof(string))]
[MemberArgument("Length", typeof(int))]
public class RightFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return (await new AsyncResultDictionaryBuilder()
            .Add<string>(context, 0, "StringExpression", token)
            .Add<int>(context, 1, "Length", token)
            .BuildAsync().ConfigureAwait(false))
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
