namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IExpression[] _expressions;

    public ExpressionEvaluator(IEnumerable<IExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions.OrderBy(x => x.Order).ToArray();
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Validate<object?>()
            .OnSuccess(() => _expressions
                .Select(x => x.Evaluate(context))
                .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Invalid<object?>($"Unknown expression type found in fragment: {context.Expression}"));
    }

    public Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        return context.Validate<T>()
            .OnSuccess(() => _expressions
                .Select(x => x is IExpression<T> typedExpression
                    ? typedExpression.EvaluateTyped(context)
                    : x.Evaluate(context).TryCastAllowNull<T>())
                .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Invalid<T>($"Unknown expression type found in fragment: {context.Expression}"));
    }

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        var validationResult = context.Validate<ExpressionParseResult>();
        if (!validationResult.IsSuccessful())
        {
            return new ExpressionParseResultBuilder().FillFromResult(validationResult);
        }

        var expression = _expressions
            .Select(x => x.Parse(context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue);

        return expression is null
            ? new ExpressionParseResultBuilder()
                .WithStatus(ResultStatus.Invalid)
                .WithErrorMessage($"Unknown expression type found in fragment: {context.Expression}")
            : expression;
    }
}
