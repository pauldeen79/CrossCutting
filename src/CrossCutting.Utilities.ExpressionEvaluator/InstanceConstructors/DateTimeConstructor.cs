namespace CrossCutting.Utilities.ExpressionEvaluator.InstanceConstructors;

[MemberName(nameof(DateTime))]
[MemberResultType(typeof(DateTime))]
[MemberArgument("Year", typeof(int))]
[MemberArgument("Month", typeof(int))]
[MemberArgument("Day", typeof(int))]
[MemberArgument("Hour", typeof(int))]
[MemberArgument("Minute", typeof(int))]
[MemberArgument("Second", typeof(int))]
public class DateTimeConstructor : IConstructor
{
    public async Task<Result<object?>> EvaluateAsync(FunctionCallContext context, CancellationToken token)
        => (await new AsyncResultDictionaryBuilder()
            .Add<int>(context, 0, "Year", token)
            .Add<int>(context, 1, "Month", token)
            .Add<int>(context, 2, "Day", token)
            .Add<int>(context, 3, "Hour", token)
            .Add<int>(context, 4, "Minute", token)
            .Add<int>(context, 5, "Second", token)
            .Build().ConfigureAwait(false))
            .OnSuccess(results => Result.WrapException<object?>(() => new DateTime(results.GetValue<int>("Year"), results.GetValue<int>("Month"), results.GetValue<int>("Day"), results.GetValue<int>("Hour"), results.GetValue<int>("Minute"), results.GetValue<int>("Second"), DateTimeKind.Unspecified)));
}
