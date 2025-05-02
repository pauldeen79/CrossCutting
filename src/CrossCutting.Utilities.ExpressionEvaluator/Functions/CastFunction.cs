namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberArgument("Type", typeof(Type))]
[MemberArgument("Expression", typeof(object))]
public class CastFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<Type>(context, 0, "Type")
            .Add(context, 1, "Expression")
            .Build()
            .OnSuccess(results =>
            {
                var method = typeof(CastFunction).GetMethod(nameof(CastHelper), BindingFlags.Static | BindingFlags.Public);
                var generic = method.MakeGenericMethod(results.GetValue<Type>("Type"));
                return Result.WrapException(() => Result.Success<object?>(generic.Invoke(null, [results.GetValue("Expression")!])));
            });

    public static T CastHelper<T>(object value) => (T)value;
}
