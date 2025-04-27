namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionArgument("Type", typeof(Type))]
[FunctionArgument("Expression", typeof(object))]
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
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    return Result.Success<object?>(generic.Invoke(null, [results.GetValue("Expression")!]));
                }
                catch (Exception ex)
                {
                    return Result.Error<object?>(ex, "Could not cast value to target type, see exception for more details");
                }
#pragma warning restore CA1031 // Do not catch general exception types
            });

    public static T CastHelper<T>(object value) => (T)value;
}
