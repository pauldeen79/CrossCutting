namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(bool))]
[MemberArgument(Constants.Expression, typeof(object))]
public class IsNullFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetArgumentValueResult(0, Constants.Expression).Transform<object?>(result => result is null);
    }
}
