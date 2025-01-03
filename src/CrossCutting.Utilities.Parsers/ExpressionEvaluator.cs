namespace CrossCutting.Utilities.Parsers;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IEnumerable<IExpression> _expressions;

    public ExpressionEvaluator(IEnumerable<IExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions;
    }

    public Result<object?> Evaluate(string expression, IFormatProvider formatProvider, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Invalid<object?>("Value is required");
        }

        return _expressions
            .OrderBy(x => x.Order)
            .Select(x => x.Evaluate(expression, formatProvider, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.NotSupported<object?>($"Unknown expression type found in fragment: {expression}");
    }

    public Result Validate(string expression, IFormatProvider formatProvider, object? context)
    {
        if (string.IsNullOrEmpty(expression))
        {
            return Result.Invalid("Value is required");
        }

        return _expressions
            .OrderBy(x => x.Order)
            .Select(x => x.Validate(expression, formatProvider, context))
            .FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Invalid($"Unknown expression type found in fragment: {expression}");
    }
}
