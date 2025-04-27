namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("YearsToAdd", typeof(int))]
public class AddYearsFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "YearsToAdd")
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<DateTime>("DateTimeExpression").AddYears(results.GetValue<int>("YearsToAdd"))));
}
