namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

public sealed class OtherExpression : IExpression
{
    private readonly ExpressionEvaluatorContext _context;

    private string Expression { get; }

    public OtherExpression(ExpressionEvaluatorContext context, string expression)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        _context = context;
        Expression = expression;
    }

    public Task<Result<object?>> EvaluateAsync()
        => _context.EvaluateAsync(Expression);

    public Task<ExpressionParseResult> ParseAsync()
        => _context.ParseAsync(Expression);
}
