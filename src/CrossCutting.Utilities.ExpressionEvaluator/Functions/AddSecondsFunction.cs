namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("SecondsToAdd", typeof(int))]
public class AddSecondsFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "SecondsToAdd")
            .Build()
            .OnSuccess(results => Result.Success(results.GetValue<DateTime>("DateTimeExpression").AddSeconds(results.GetValue<int>("SecondsToAdd"))));
}
