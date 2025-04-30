namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberArgument("Expression", typeof(object))]
public class CoalesceFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        foreach (var argument in context.FunctionCall.Arguments)
        {
            var result = context.Context.Evaluate(argument);
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
