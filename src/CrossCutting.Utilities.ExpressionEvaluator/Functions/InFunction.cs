namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(bool))]
[LanguageFunctionAttribute]
[MemberAllowAllArguments]
public class InFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var leftValueResult = (await context.GetArgumentValueResultAsync(0, Constants.Expression, token).ConfigureAwait(false));
        if (!leftValueResult.IsSuccessful())
        {
            return leftValueResult;
        }

        var rightValues = new List<object?>();
        foreach (var argument in context.FunctionCall.Arguments.Skip(1))
        {
            var result = await context.Context.EvaluateAsync(argument, token).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }
            rightValues.Add(result.Value);
        }

        return Result.Success<object?>(leftValueResult.Value.In(rightValues));
    }
}
