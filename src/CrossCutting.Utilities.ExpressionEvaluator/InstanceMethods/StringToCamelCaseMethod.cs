namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(StringExtensions.ToCamelCase))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToCamelCaseMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var instanceResult = context.GetInstanceValueResult<string>();
        if (!instanceResult.IsSuccessful())
        {
            return Result.FromExistingResult<object?>(instanceResult);
        }

        var sourceValue = instanceResult.GetValueOrThrow();

        return Result.Success<object?>(sourceValue.ToCamelCase(context.Context.Settings.FormatProvider.ToCultureInfo()));
    }
}
