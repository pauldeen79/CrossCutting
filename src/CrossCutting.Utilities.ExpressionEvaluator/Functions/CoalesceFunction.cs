namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

public class CoalesceFunction : IDynamicDescriptorsFunction
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

    public IEnumerable<FunctionDescriptor> GetDescriptors()
    {
        yield return new FunctionDescriptorBuilder()
            .WithName("Coalesce")
            .WithDescription("Gets the first expression that is not null, or null when not found")
            .WithFunctionType(typeof(CoalesceFunction))
            .AddArguments(new FunctionDescriptorArgumentBuilder()
                .WithName("expression")
                .WithType(typeof(object))
                .WithIsRequired(true)
                .WithDescription("One or more expressions, from which the first expression that is not null will be returned"));
    }
}
