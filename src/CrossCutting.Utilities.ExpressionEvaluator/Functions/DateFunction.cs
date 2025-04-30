namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(DateTime))]
[FunctionArgument("Year", typeof(int))]
[FunctionArgument("Month", typeof(int))]
[FunctionArgument("Day", typeof(int))]
public class DateFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
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
                    return Result.Success<object?>(new DateTime(results.GetValue<int>("Year"), results.GetValue<int>("Month"), results.GetValue<int>("Day"), 0, 0, 0, DateTimeKind.Unspecified));
                }
                catch (Exception ex)
                {
                    return Result.Error<object?>(ex, "Could not create date, see exception for more details");
                }
#pragma warning restore CA1031 // Do not catch general exception types
            });
}
