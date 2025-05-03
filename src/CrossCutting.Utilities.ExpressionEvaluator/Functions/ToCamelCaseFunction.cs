namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(string))]
[MemberType(MemberType.Method)]
[MemberInstanceType(typeof(string))]
public class ToCamelCaseFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceValueResult = context.GetInstanceValueResult<string>();
        if (!instanceValueResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceValueResult);
        }
        
        return Result.Success<object?>(instanceValueResult.GetValueOrThrow().ToCamelCase(context.Context.Settings.FormatProvider.ToCultureInfo()));
    }
}
