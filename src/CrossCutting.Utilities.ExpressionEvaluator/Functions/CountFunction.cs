namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionArgument("EnumerableExpression", typeof(IEnumerable))]
public class CountFunction : IFunction<int>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<int> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetArgumentValueResult<IEnumerable>(0, "EnumerableExpression")
            .Transform(enumerable => enumerable.OfType<object?>().Count());
    }
}
