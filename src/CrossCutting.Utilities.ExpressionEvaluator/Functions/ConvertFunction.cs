namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("Convert")]
[FunctionArgument("Type", typeof(Type))]
[FunctionArgument("Expression", typeof(object))]
public class ConvertFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<Type>(context, 0, "Type")
            .Add(context, 1, "Expression")
            .Build()
            .OnSuccess(results =>
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    return Result.Success<object?>(Convert.ChangeType(results.GetValue("Expression")!, results.GetValue<Type>("Type")));
                }
                catch (Exception ex)
                {
                    return Result.Error<object?>(ex, "Could not convert value to target type, see exception for more details");
                }
#pragma warning restore CA1031 // Do not catch general exception types
            });
}
