namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(DateTime))]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("DaysToAdd", typeof(int))]
public class AddDaysFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "DaysToAdd")
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>("DateTimeExpression").AddDays(results.GetValue<int>("DaysToAdd"))));
}
