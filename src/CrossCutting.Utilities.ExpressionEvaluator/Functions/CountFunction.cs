namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(int))]
[MemberArgument("EnumerableExpression", typeof(IEnumerable))]
public class CountFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetArgumentValueResult<IEnumerable>(0, "EnumerableExpression")
            .Transform<object?>(enumerable => enumerable.OfType<object?>().Count());
    }
}
