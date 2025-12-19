namespace CrossCutting.Utilities.ExpressionEvaluator.Expressions;

internal sealed class OtherExpression : IExpression
{
    private readonly ExpressionEvaluatorContext _context;

    public string SourceExpression { get; }

    public OtherExpression(ExpressionEvaluatorContext context, string expression)
    {
        ArgumentGuard.IsNotNull(context, nameof(context));
        ArgumentGuard.IsNotNull(expression, nameof(expression));

        _context = context;
        SourceExpression = expression;
    }

    public Task<Result<object?>> EvaluateAsync(ExpressionEvaluatorContext context, CancellationToken token)
        => context.EvaluateAsync(SourceExpression, token);

    public Task<ExpressionParseResult> ParseAsync(CancellationToken token)
        => _context.ParseAsync(SourceExpression, token);

    public IEvaluatableBuilder ToBuilder() => throw new NotSupportedException();
}
