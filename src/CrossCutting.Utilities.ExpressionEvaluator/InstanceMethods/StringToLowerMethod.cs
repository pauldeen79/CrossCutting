namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.ToLower))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToLowerMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<string>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().ToLower(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
