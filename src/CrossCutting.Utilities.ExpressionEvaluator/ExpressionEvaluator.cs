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

        if (string.IsNullOrEmpty(context.Expression))
        {
            return Result.Invalid<object?>("Value is required");
        }

        var expressionContext = new ExpressionEvaluatorContext(context.Expression, context.Settings, context.Context, this, 0);

        return _expressions
            .Select(x => x.Evaluate(expressionContext))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<object?>($"Unknown expression type found in fragment: {context.Expression}");
    }

    public Result<T> EvaluateTyped<T>(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (string.IsNullOrEmpty(context.Expression))
        {
            return Result.Invalid<T>("Value is required");
        }

        var expressionContext = new ExpressionEvaluatorContext(context.Expression, context.Settings, context.Context, this, 0);

        return _expressions
            .Select(x => x is IExpression<T> typedExpression
                ? typedExpression.EvaluateTyped(expressionContext)
                : x.Evaluate(expressionContext).TryCast<T>())
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<T>($"Unknown expression type found in fragment: {context.Expression}");
    }

    public Result<Type> Validate(ExpressionEvaluatorContext context)
    {
        context = ArgumentGuard.IsNotNull(context, nameof(context));

        if (string.IsNullOrEmpty(context.Expression))
        {
            return Result.Invalid<Type>("Value is required");
        }

        var expressionContext = new ExpressionEvaluatorContext(context.Expression, context.Settings, context, this, 0);

        return _expressions
            .Select(x => x.Validate(expressionContext))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<Type>($"Unknown expression type found in fragment: {context.Expression}");
    }
}
