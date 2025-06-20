﻿namespace CrossCutting.Utilities.Parsers;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IEnumerable<IExpression> _expressions;

    public ExpressionEvaluator(IEnumerable<IExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions;
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
            .WhenNull(ResultStatus.NotSupported, $"Unknown expression type found in fragment: {expression}");
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
            .WhenNull(ResultStatus.Invalid, $"Unknown expression type found in fragment: {expression}");
    }
}
