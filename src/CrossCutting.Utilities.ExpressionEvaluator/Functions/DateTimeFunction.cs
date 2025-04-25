namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("DateTime")]
[FunctionArgument("Year", typeof(int))]
[FunctionArgument("Month", typeof(int))]
[FunctionArgument("Day", typeof(int))]
[FunctionArgument("Hour", typeof(int))]
[FunctionArgument("Minute", typeof(int))]
[FunctionArgument("Second", typeof(int))]
public class DateTimeFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<int>(context, 0, "Year")
            .Add<int>(context, 1, "Month")
            .Add<int>(context, 2, "Day")
            .Add<int>(context, 3, "Hour")
            .Add<int>(context, 4, "Minute")
            .Add<int>(context, 5, "Second")
            .Build()
            .OnSuccess(results =>
            {
#pragma warning disable CA1031 // Do not catch general exception types
                try
                {
                    return Result.Success(new DateTime(results.GetValue<int>("Year"), results.GetValue<int>("Month"), results.GetValue<int>("Day"), results.GetValue<int>("Hour"), results.GetValue<int>("Minute"), results.GetValue<int>("Second"), DateTimeKind.Unspecified));
                }
                catch (Exception ex)
                {
                    return Result.Error<DateTime>(ex, "Could not create datetime, see exception for more details");
                }
#pragma warning restore CA1031 // Do not catch general exception types
            });
}
