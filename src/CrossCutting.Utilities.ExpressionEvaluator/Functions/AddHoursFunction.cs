namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("AddHours")]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("HoursToAdd", typeof(int))]
public class AddHoursFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add("DateTimeExpression", () => context.GetArgumentDateTimeValueResult(0, "DateTimeExpression"))
            .Add("HoursToAdd", () => context.GetArgumentInt32ValueResult(1, "HoursToAdd"))
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<DateTime>("DateTimeExpression").AddHours(results.GetValue<int>("HoursToAdd"))));
}
