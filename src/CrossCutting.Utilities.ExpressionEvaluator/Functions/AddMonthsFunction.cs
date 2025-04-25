namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("AddMonths")]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("MonthsToAdd", typeof(int))]
public class AddMonthsFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "MonthsToAdd")
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<DateTime>("DateTimeExpression").AddMonths(results.GetValue<int>("MonthsToAdd"))));
}
