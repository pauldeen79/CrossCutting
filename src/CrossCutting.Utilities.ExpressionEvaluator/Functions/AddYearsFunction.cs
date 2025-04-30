namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionResultType(typeof(DateTime))]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
[FunctionArgument("YearsToAdd", typeof(int))]
public class AddYearsFunction : IFunction
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => new ResultDictionaryBuilder()
            .Add<DateTime>(context, 0, "DateTimeExpression")
            .Add<int>(context, 1, "YearsToAdd")
            .Build()
            .OnSuccess(results => Result.Success<object?>(results.GetValue<DateTime>("DateTimeExpression").AddYears(results.GetValue<int>("YearsToAdd"))));
}
