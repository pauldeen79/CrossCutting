namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(DateTime))]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("SecondsToAdd", typeof(int))]
public class AddSecondsFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
       => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "SecondsToAdd")
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>("DateTimeExpression").AddSeconds(results.GetValue<int>("SecondsToAdd"))));
}
