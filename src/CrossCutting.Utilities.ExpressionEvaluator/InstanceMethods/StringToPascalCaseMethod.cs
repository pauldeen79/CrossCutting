namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(StringExtensions.ToPascalCase))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToPascalCaseMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<string>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().ToPascalCase(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
