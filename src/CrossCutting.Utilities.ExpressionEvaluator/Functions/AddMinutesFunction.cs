namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("AddMinutes")]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("MinutesToAdd", typeof(int))]
public class AddMinutesFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add("DateTimeExpression", () => context.GetArgumentDateTimeValueResult(0, "DateTimeExpression"))
            .Add("MinutesToAdd", () => context.GetArgumentInt32ValueResult(1, "MinutesToAdd"))
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<DateTime>("DateTimeExpression").AddMinutes(results.GetValue<int>("MinutesToAdd"))));
}
