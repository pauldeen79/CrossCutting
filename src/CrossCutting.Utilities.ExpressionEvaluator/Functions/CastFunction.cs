namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberArgument("Type", typeof(Type))]
[MemberArgument("Expression", typeof(object))]
public class CastFunction : IFunction
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context)
        => (await new AsyncResultDictionaryBuilder()
            .Add<Type>(context, 0, "Type")
            .Add(context, 1, "Expression")
            .Build().ConfigureAwait(false))
            .OnSuccess(results =>
            {
                var method = typeof(CastFunction).GetMethod(nameof(CastHelper), BindingFlags.Static | BindingFlags.Public);
                var generic = method.MakeGenericMethod(results.GetValue<Type>("Type"));
                return Result.WrapException(() => Result.Success<object?>(generic.Invoke(null, [results.GetValue("Expression")!])));
            });

    public static T CastHelper<T>(object value) => (T)value;
}
