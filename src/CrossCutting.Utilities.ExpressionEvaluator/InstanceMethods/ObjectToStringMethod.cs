namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceMethods;

[MemberName(nameof(object.ToString))]
[MemberInstanceType(typeof(object))]
[MemberResultType(typeof(string))]
public class ObjectToStringMethod : IMethod
{
    public Result<object?> Evaluate(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return new ResultDictionaryBuilder()
            .Add(Constants.Instance, () => context.GetInstanceValueResult<object>())
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<object?>(Constants.Instance).ToString(context.Context.Settings.FormatProvider)));
    }
}
