namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(DateTime))]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("MonthsToAdd", typeof(int))]
public class AddMonthsFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "MonthsToAdd")
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>("DateTimeExpression").AddMonths(results.GetValue<int>("MonthsToAdd"))));
}
