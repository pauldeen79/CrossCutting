namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

internal sealed class OtherExpression : ExpressionBase
{
    private readonly ExpressionEvaluatorContext _context;

    internal string Expression { get; }

    public OtherExpression(ExpressionEvaluatorContext context, string expression)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        _context = context;
        Expression = expression;
    }

    public override Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => _context.EvaluateAsync(Expression, token);

    public override Task<ExpressionParseResult> ParseAsync(CancellationToken token)
        => _context.ParseAsync(Expression, token);
}
