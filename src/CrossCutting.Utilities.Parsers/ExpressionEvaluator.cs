namespace CrossCutting.Utilities.Parsers;

public class ExpressionEvaluator : IExpressionEvaluator
{
    private readonly IEnumerable<IExpression> _expressions;

    public ExpressionEvaluator(IEnumerable<IExpression> expressions)
    {
        ArgumentGuard.IsNotNull(expressions, nameof(expressions));

        _expressions = expressions;
    }

    public Result<object?> Evaluate(string expression, ExpressionEvaluatorSettings settings, object? context)
         => Result.Validate<object?>(() => !string.IsNullOrEmpty(expression), "Value is required")
            .OnSuccess(() =>
            {
                var expressionContext = new ExpressionEvaluatorContext(expression, settings, context, this);

                return _expressions
                    .Select(x => x.Evaluate(expressionContext))
                    .WhenNotContinue(() => Result.NotSupported<object?>($"Unknown expression type found in fragment: {expression}"));
            });

    public Result<Type> Validate(string expression, ExpressionEvaluatorSettings settings, object? context)
         => Result.Validate<Type>(() => !string.IsNullOrEmpty(expression), "Value is required")
            .OnSuccess(() =>
            {
                var expressionContext = new ExpressionEvaluatorContext(expression, settings, context, this);

                return _expressions
                    .Select(x => x.Validate(expressionContext))
                    .WhenNotContinue(() => Result.Invalid<Type>($"Unknown expression type found in fragment: {expression}"));
            });
}
