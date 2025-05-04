namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(StringExtensions.ToCamelCase))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToCamelCaseMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<string>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().ToCamelCase(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
