namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(StringExtensions.ToCamelCase))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToCamelCaseMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<string>())
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<string>(Constants.Instance).ToCamelCase(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
