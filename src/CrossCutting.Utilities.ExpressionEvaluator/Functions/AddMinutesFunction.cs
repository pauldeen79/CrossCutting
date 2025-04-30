namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(DateTime))]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("MinutesToAdd", typeof(int))]
public class AddMinutesFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "MinutesToAdd")
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>("DateTimeExpression").AddMinutes(results.GetValue<int>("MinutesToAdd"))));
}
