namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public class PrimitiveExpression : IExpression
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public PrimitiveExpression(IDateTimeProvider dateTimeProvider)
    {
        ArgumentGuard.IsNotNull(dateTimeProvider, nameof(dateTimeProvider));
        _dateTimeProvider = dateTimeProvider;
    }

    public int Order => 10;

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Expression switch
        {
            "true" => Result.Success<object?>(true),
            "false" => Result.Success<object?>(false),
            "null" => Result.Success<object?>(null),
            "context" => Result.Success(context.Context),
            "DateTime.Now" => Result.Success<object?>(_dateTimeProvider.GetCurrentDateTime()),
            "DateTime.Today" => Result.Success<object?>(_dateTimeProvider.GetCurrentDateTime().Date),
            _ => Result.Continue<object?>()
        };
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var result = new ExpressionParseResultBuilder()
            .WithSourceExpression(context.Expression)
            .WithExpressionType(typeof(PrimitiveExpression));

        return context.Expression switch
        {
            "true" or "false" => result.WithResultType(typeof(bool)).WithStatus(ResultStatus.Ok),
            // for null, the result type is null
            "null" => result.WithStatus(ResultStatus.Ok),
            "context" => result.WithResultType(context.Context?.GetType()).WithStatus(ResultStatus.Ok),
            "DateTime.Now" or "DateTime.Today" => result.WithResultType(typeof(DateTime)).WithStatus(ResultStatus.Ok),
            _ => result.WithStatus(ResultStatus.Continue)
        };
    }
}
