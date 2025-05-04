namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(string.ToUpper))]
[MemberInstanceType(typeof(string))]
[MemberResultType(typeof(string))]
public class StringToUpperMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<string>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().ToUpper(context.Context.Settings.FormatProvider.ToCultureInfo())));
    }
}
