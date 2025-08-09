namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class OtherExpression : IExpression
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

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => _context.EvaluateAsync(Expression, token);

    public Task<ExpressionParseResult> ParseAsync(CancellationToken token)
        => _context.ParseAsync(Expression, token);
}
