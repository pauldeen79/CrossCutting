namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberArgument("Expression", typeof(object))]
[MemberAllowAllArguments]
public class CoalesceFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        foreach (var argument in context.FunctionCall.Arguments)
        {
            var result = await context.Context.EvaluateAsync(argument, token).ConfigureAwait(false);
            if (!result.IsSuccessful())
            {
                return result;
            }

            if (result.Value is not null)
            {
                return result;
            }
        }

        return Result.NoContent<object?>();
    }
}
