namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(string))]
[FunctionArgument("StringExpression", typeof(object))]
public class ToLowerCaseFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetArgumentValueResult<string>(0, "StringExpression")
            .Transform(result => Result.Success<object?>(result.GetValueOrThrow().ToLower(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
