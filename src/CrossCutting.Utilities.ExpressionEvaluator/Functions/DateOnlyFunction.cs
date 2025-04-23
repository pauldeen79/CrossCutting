namespace CrossCutting.Utilities.ExpressionEvaluator.Functions;

[FunctionName("Date")]
[FunctionArgument("DateTimeExpression", typeof(DateTime))]
public class DateOnlyFunction : IFunction<DateTime>
{
    public Result<object?> Evaluate(FunctionCallContext context)
        => EvaluateTyped(context).TryCastAllowNull<object?>();

    public Result<DateTime> EvaluateTyped(FunctionCallContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));
        
        return context.GetArgumentDateTimeValueResult(0, "DateTimeExpression")
            .OnSuccess(result => Result.Success(result.Value.Date));
    } 
}