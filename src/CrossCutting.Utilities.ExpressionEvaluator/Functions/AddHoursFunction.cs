namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(DateTime))]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("HoursToAdd", typeof(int))]
public class AddHoursFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "HoursToAdd")
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>("DateTimeExpression").AddHours(results.GetValue<int>("HoursToAdd"))));
}
