namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[MemberName(nameof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument("Year", typeof(int))]
[MemberArgument("Month", typeof(int))]
[MemberArgument("Day", typeof(int))]
public class DateConstructor : IConstructor
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
       => (await new AsyncResultDictionaryBuilder()
            .Add<int>(context, 0, "Year", token)
            .Add<int>(context, 1, "Month", token)
            .Add<int>(context, 2, "Day", token)
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.WrapException<object?>(() => new DateTime(results.GetValue<int>("Year"), results.GetValue<int>("Month"), results.GetValue<int>("Day"), 0, 0, 0, DateTimeKind.Unspecified)));
}
