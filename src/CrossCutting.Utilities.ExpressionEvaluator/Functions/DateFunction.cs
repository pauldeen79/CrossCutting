namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("Date")]
[FunctionArgument("Year", typeof(int))]
[FunctionArgument("Month", typeof(int))]
[FunctionArgument("Day", typeof(int))]
public class DateFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<int>(context, 0, "Year")
            .Add<int>(context, 1, "Month")
            .Add<int>(context, 2, "Day")
            .Build()
            .OnSuccess(results =>
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    return Result.Success(new DateTime(results.GetValue<int>("Year"), results.GetValue<int>("Month"), results.GetValue<int>("Day"), 0, 0, 0, DateTimeKind.Unspecified));
                }
                catch (Exception ex)
                {
                    return Result.Error<DateTime>(ex, "Could not create date, see exception for more details");
                }
#pragma warning restore CA1031 // Do not catch general exception types
            });
}
