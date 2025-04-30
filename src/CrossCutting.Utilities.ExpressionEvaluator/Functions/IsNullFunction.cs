namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(bool))]
[FunctionArgument("Expression", typeof(object))]
public class IsNullFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetArgumentValueResult(0, "Expression")
            .Transform(result => Result.Success<object?>(result.Value is null));
    }
}
