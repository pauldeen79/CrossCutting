namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(string))]
[MemberType(MemberType.Method)]
[MemberInstanceType(typeof(string))]
public class ToCamelCaseFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context
            .GetInstanceValueResult<string>()
            .Transform(result => Result.Success<object?>(result.GetValueOrThrow().ToCamelCase(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
