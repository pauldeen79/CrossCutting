namespace CrossCutting.Utilities.ExpressionEvaluator;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IExpression[] _expressions;

    public ExpressionEvaluator(IEnumerable<IExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions.OrderBy(x => x.Order).ToArray();
    }

    public Result<object?> Evaluate(string expression, ExpressionEvaluatorSettings settings, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Invalid<object?>("Value is required");
        }

        var expressionContext = new ExpressionEvaluatorContext(expression, settings, context, this);

        return _expressions
            .Select(x => x.Evaluate(expressionContext))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown expression type found in fragment: {expression}");
    }

    public Result<Type> Validate(string expression, ExpressionEvaluatorSettings settings, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Invalid<Type>("Value is required");
        }

        var expressionContext = new ExpressionEvaluatorContext(expression, settings, context, this);

        return _expressions
            .Select(x => x.Validate(expressionContext))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid<Type>($"Unknown expression type found in fragment: {expression}");
    }
}
