namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionArgument("DateTimeExpression", typeof(DateTime))]
public class DatePartFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        
        return context.GetArgumentValueResult<DateTime>(0, "DateTimeExpression")
            .OnSuccess(result => Result.Success(result.Value.Date));
    } 
}
