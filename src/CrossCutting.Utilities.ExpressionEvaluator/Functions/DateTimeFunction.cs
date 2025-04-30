namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberResultType(typeof(DateTime))]
[MemberArgument("Year", typeof(int))]
[MemberArgument("Month", typeof(int))]
[MemberArgument("Day", typeof(int))]
[MemberArgument("Hour", typeof(int))]
[MemberArgument("Minute", typeof(int))]
[MemberArgument("Second", typeof(int))]
public class DateTimeFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
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
                    return Result.Success<object?>(new DateTime(results.GetValue<int>("Year"), results.GetValue<int>("Month"), results.GetValue<int>("Day"), results.GetValue<int>("Hour"), results.GetValue<int>("Minute"), results.GetValue<int>("Second"), DateTimeKind.Unspecified));
                }
                catch (Exception ex)
                {
                    return Result.Error<object?>(ex, "Could not create datetime, see exception for more details");
                }
#pragma warning restore CA1031 // Do not catch general exception types
            });
}
