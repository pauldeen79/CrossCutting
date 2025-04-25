namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("AddDays")]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("DaysToAdd", typeof(int))]
public class AddDaysFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "DaysToAdd")
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<DateTime>("DateTimeExpression").AddDays(results.GetValue<int>("DaysToAdd"))));
}
