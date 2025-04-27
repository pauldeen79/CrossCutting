namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("MinutesToAdd", typeof(int))]
public class AddMinutesFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "MinutesToAdd")
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<DateTime>("DateTimeExpression").AddMinutes(results.GetValue<int>("MinutesToAdd"))));
}
