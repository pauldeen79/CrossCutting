namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(string))]
[MemberArgument("StringExpression", typeof(object))]
public class ToCamelCaseFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetArgumentValueResult<string>(0, "StringExpression")
            .Transform(result => Result.Success<object?>(result.GetValueOrThrow().ToCamelCase(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
