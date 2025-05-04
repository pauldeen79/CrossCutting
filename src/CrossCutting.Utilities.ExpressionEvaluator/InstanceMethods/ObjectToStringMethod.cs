namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(object.ToString))]
[MemberInstanceType(typeof(object))]
[MemberResultType(typeof(string))]
public class ObjectToStringMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.GetInstanceValueResult<object>()
            .Transform(x => Result.Success<object?>(x.GetValueOrThrow().ToString(context.Context.Settings.FormatProvider)));
    }
}
