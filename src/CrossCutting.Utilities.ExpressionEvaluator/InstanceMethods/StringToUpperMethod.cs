namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.ToUpper))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToUpperMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<string>())
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<string>(Constants.Instance).ToUpper(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
